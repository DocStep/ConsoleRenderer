using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using FFmpeg.AutoGen;
using NAudio.Wave;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Channels;


namespace ConsoleRenderer {
    public class Video {

        public Engine engine;
        //public Renderer renderer;

        MediaFile file;
        //Bitmap frame;
        byte[] bufferRaw_bytes;
        Pixel[,] bufferRaw;
        Pixel[,] bufferLowRes;
        int videoHeightRaw;
        int videoWidthRaw;
        int videoHeightLowRes;
        int videoWidthLowRes;
        float scaleW;
        float scaleH;
        int avgSamples;
        float _1_avgSamples;
        int bpp;

        public Video (Engine engine, string path, int pixelsToCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
            engine.state = States.video;

            FFmpegLoader.FFmpegPath = @"D:\ffmpeg\bin";

            file = MediaFile.Open(path);
            videoHeightRaw = file.Video.Info.FrameSize.Height;
            videoWidthRaw = file.Video.Info.FrameSize.Width;
            videoHeightLowRes = videoHeightRaw/pixelsToCell;
            videoWidthLowRes = videoWidthRaw/pixelsToCell;
            scaleH = (float)videoHeightRaw/videoHeightLowRes;
            scaleW = (float)videoWidthRaw/videoWidthLowRes;
            avgSamples = (int)(scaleH*scaleW);
            _1_avgSamples = 1f/avgSamples;
            bpp = 3;
            engine.fpsMax = file.Video.Info.AvgFrameRate;

            engine.renderer = new Renderer(videoHeightLowRes, 2*videoWidthLowRes, colors);
            engine.renderer.debugger.state = engine.state.ToString();
            engine.renderer.title = Path.GetFileName(path);
            engine.renderer.forceAscii = useAscii;
            engine.renderer.debugger.framesQueue = 0;

            bufferRaw_bytes = new byte[videoHeightRaw*videoWidthRaw*bpp];
            bufferRaw = new Pixel[videoHeightRaw, videoWidthRaw];
            bufferLowRes = new Pixel[videoHeightLowRes, videoWidthLowRes];

            /// Audio
            try { /// I know it sucks
                AudioFileReader audioFile = new AudioFileReader(path);
                WaveOutEvent outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Volume = 0.1f;
                outputDevice.Play();
            } catch (Exception _) { }
            
            this.engine = engine;
        }



        public void Keys () {
            if (Input.GetKeyDown('Q')) {
                engine.Menu();
            }
            if (Input.GetKeyDown('X')) {
                engine.renderer.debugger.debug = !engine.renderer.debugger.debug;
            }
        }
        public void Start () {

        }
        public void Update () {
            //int bytesPerPixel = 4;
            //if (file.Video.TryGetNextFrame(frameBufferRaw)) {
            if (file.Video.TryGetNextFrame(bufferRaw_bytes)) {
                BufferBytesToPixels(bufferRaw_bytes, bufferRaw, bpp);
                ResizeBufferPixels(bufferRaw, bufferLowRes, average: true);

                if (engine.renderer.framesTotal % 10 == 0) {
                    string path = @$"D:/temp/frame{engine.renderer.framesTotal/10}.png";
                    //SaveFrame(frameBufferRaw, videoWidth, videoHeight, path);
                    //SaveFrame(bufferLowRes, path);
                }

                if (engine.renderer.forceAscii) {
                    engine.renderer.VideoToAscii(bufferLowRes);
                } else {
                    engine.renderer.VideoToArrays(bufferLowRes);
                }
            } else {
                engine.engineWork = false;
                //Environment.Exit(0);
            }
        }
        public void UpdateSkip () {
            if (!file.Video.TryGetNextFrame(bufferRaw_bytes)) {
                /// <> <ch> to handle broken frame
                engine.engineWork = false;
                Environment.Exit(56);
            }
        }


        void BufferBytesToPixels (byte[] raw, Pixel[,] buffer, int bpp) {
            for (int y = 0; y < videoHeightRaw; y++) {
                for (int x = 0; x < videoWidthRaw; x++) {
                    int idx = (y*videoWidthRaw + x)*bpp;
                    buffer[y, x] = new Pixel(raw[idx + 0], raw[idx + 1], raw[idx + 2]);
                    //buffer[x, y] = new Pixel((byte)new Random().Next(256));
                }
            }
        }
        void ResizeBufferPixels (Pixel[,] src, Pixel[,] dst, bool average = true) {
            for (int y = 0; y < videoHeightLowRes; y++) {
                int srcY = (int)(scaleH*y);
                for (int x = 0; x < videoWidthLowRes; x++) {
                    int srcX = (int)(scaleW*x);
                    if (average) {
                        long R = 0;
                        long G = 0;
                        long B = 0;
                        for (int avgH = 0; avgH < scaleH; avgH++) {
                            for (int avgW = 0; avgW < scaleW; avgW++) {
                                Pixel pixel = src[srcY, srcX];
                                R += pixel.R;
                                G += pixel.G;
                                B += pixel.B;
                            }
                        }
                        dst[y, x].R = (byte)(_1_avgSamples*R);
                        dst[y, x].G = (byte)(_1_avgSamples*G);
                        dst[y, x].B = (byte)(_1_avgSamples*B);
                    } else {
                        dst[y, x] = src[srcY, srcX];
                    }
                }
            }
        }


        public static void SaveFrame (byte[] rawBuffer, int height, int width, string path) {
            Image image = Image.LoadPixelData<Rgb24>(rawBuffer, width, height);
            image.Save(path);
        }
        public static void SaveFrame (Pixel[,] buffer, string path) {
            int h = buffer.GetLength(0);
            int w = buffer.GetLength(1);

            var image = new Image<Rgb24>(w, h);

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++) {
                    var p = buffer[y, x];
                    image[x, y] = new Rgb24(p.R, p.G, p.B);
                }

            image.Save(path);
        }


        void DownsizeFrame (byte[] src, int srcW, int srcH, byte[] dst, int dstW, int dstH, int bpp) {
            for (int y = 0; y < dstH; y++) {
                int srcY = y * srcH / dstH;
                for (int x = 0; x < dstW; x++) {
                    int srcX = x*srcW/dstW;
                    int srcIndex = (srcY*srcW + srcX)*bpp;
                    int dstIndex = (y*dstW + x)*bpp;

                    for (int i = 0; i < bpp; i++)
                        dst[dstIndex + i] = src[srcIndex + i];
                }
            }
        }



        public void Exit () {
            //outputDevice.Stop();
            //engine.isSelfEnd = true;

        }

    }
}
