using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using NAudio.Wave;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
//using System.Drawing;


namespace ConsoleRenderer;

public class Video : Scene {
    //public Video (string path, int pixelsPerCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
    public Video (string path, int pixelsPerCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
        if (!File.Exists(path)) Environment.Exit((int)Errors.VideoNoFile);

        Engine.state = EngineStates.Video;

        FFmpegLoader.FFmpegPath = @"D:\ffmpeg\bin";
        file = MediaFile.Open(path);
        videoHeightRaw = file.Video.Info.FrameSize.Height;
        videoWidthRaw = file.Video.Info.FrameSize.Width;
        videoHeightLowRes = videoHeightRaw/pixelsPerCell;
        videoWidthLowRes = videoWidthRaw/pixelsPerCell;
        scaleH = (float)videoHeightRaw/videoHeightLowRes;
        scaleW = (float)videoWidthRaw/videoWidthLowRes;
        avgSamples = (int)(scaleH*scaleW);
        _1_avgSamples = 1f/avgSamples;
        bpp = 3;
        Engine.fpsMax = file.Video.Info.AvgFrameRate;

        Renderer.Init(videoHeightLowRes, 2*videoWidthLowRes, colors);
        //Engine.Renderer = new Renderer(videoHeightLowRes, 2*videoWidthLowRes, colors);
        Renderer.title = Path.GetFileName(path);
        Renderer.forceAscii = useAscii;
        //Engine.Renderer.debugger.framesQueue = 0;
        //engine.renderer.blurPower = 1f;
        Renderer.cellFrameBuffer = new Pixel[videoHeightRaw, videoWidthRaw];
        Renderer.cellFrameBufferGs = new Pixel[videoHeightRaw, videoWidthRaw];


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
        } catch (Exception) { }
    }


    MediaFile file;
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


    public override void FixedUpdate () {
        if (file.Video.TryGetNextFrame(bufferRaw_bytes)) {
            BufferBytesToPixels(bufferRaw_bytes, bufferRaw, bpp);
            ResizeBufferPixels(bufferRaw, bufferLowRes, average: true);

            if (RendererDebugger.framesTotal % 10 == 0) {
                string path = @$"D:/temp/frame{RendererDebugger.framesTotal/10}.png";
                //SaveFrame(bufferRaw, videoHeightRaw, videoWidthRaw, path);
                SaveFrame(bufferLowRes, path);
            }

            if (Renderer.forceAscii) {
                Renderer.VideoToAscii(bufferLowRes);
            } else {
                Renderer.VideoToArrays(bufferLowRes);
            }
        } else {
            Engine.engineWork = false;
            //Environment.Exit(0);
        }
    }
    public override void FixedUpdate_Skip () {
        if (!file.Video.TryGetNextFrame(bufferRaw_bytes)) {
            /// <> <ch> to handle broken frame
            Engine.engineWork = false;
            Environment.Exit(200);
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


    public void SaveFrame (byte[] rawBuffer, int height, int width, string path) {
        Image image = Image.LoadPixelData<Rgb24>(rawBuffer, width, height);
        image.Save(path);
    }
    public void SaveFrame (Pixel[,] buffer, string path) {
        Image<Rgb24> image = new Image<Rgb24>(videoWidthLowRes, videoHeightLowRes);

        for (int y = 0; y < videoHeightLowRes; y++)
            for (int x = 0; x < videoWidthLowRes; x++) {
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



    public override void Inputs () {
        if (Input.GetKeyDown('Q')) {
            //engine.Menu();
            //Engine.ConsoleGame = new MenuCanvas();
        }
        if (Input.GetKeyDown('X')) {
            RendererDebugger.debug = !RendererDebugger.debug;
        }
    }

}
