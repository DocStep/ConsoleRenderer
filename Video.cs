using AForge.Video.FFMPEG;
using MediaToolkit.Model;
using NAudio.Wave;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace ConsoleRenderer {
    public class Video {

        public Engine engine;
        //public Renderer renderer;

        VideoFileReader reader;
        Bitmap frame;
        WaveOutEvent outputDevice = new WaveOutEvent();
        int videoWidth;
        int videoHeight;

        public Video ( Engine engine, string path, int pixelsToCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
            engine.state = States.video;
            reader = new VideoFileReader();
            reader.Open(path);
            videoWidth = reader.Width;
            videoHeight = reader.Height;
            engine.fpsMax = reader.FrameRate;

            engine.renderer = new Renderer(videoWidth*2/pixelsToCell, videoHeight/pixelsToCell, colors);
            engine.renderer.debugger.state = engine.state.ToString();
            engine.renderer.title = Path.GetFileName(path);
            engine.renderer.forceAscii = useAscii;
            engine.renderer.debugger.framesQueue = 0;

            var inputFile = new MediaFile { Filename = path };
            new MediaToolkit.Engine().GetMetadata(inputFile);
            if (inputFile.Metadata.AudioData != null) {
                if (inputFile.Metadata.AudioData != null) {
                    var audioFile = new AudioFileReader(path);
                    outputDevice = new WaveOutEvent();
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                }
            }

            this.engine = engine;
            //renderer = engine.renderer;
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
            frame = reader.ReadVideoFrame();

            if (frame != null) {
                if (engine.renderer.forceAscii) {
                    engine.renderer.VideoToAscii(frame);
                } else {
                    engine.renderer.VideoToArrays(frame);
                }
            } else engine.engineWork = false;
        }
        public void UpdateSkip () {
            frame = reader.ReadVideoFrame();
            if (frame == null) engine.engineWork = false;
        }
        public void Exit () {
            outputDevice.Stop();
            //engine.isSelfEnd = true;

        }

    }
}
