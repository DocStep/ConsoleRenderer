using System;
using System.Collections.Generic;


#pragma warning disable CA1416
namespace ConsoleRenderer {
    public abstract class ConsoleGame {

        public Engine engine;
        public Renderer renderer;
        public RendererDebugger debugger;


        public Engine Menu () {
            engine = new Engine(this);
            engine.Menu();

            return engine;
        }

        public Engine Video (string videoPath, int pixelsPerCell, bool useAscii) {
            engine = new Engine(this);
            engine.Video(videoPath, pixelsPerCell, DefaultValues.colorsGreys4, useAscii);

            return engine;
        }

        public Engine VideoFromText (string textPath, int width, int height, int fps) {
            engine = new Engine(this);
            engine.VideoFromText(textPath, width, height, fps);

            return engine;
        }

        public Engine CreateGame (int width, int height, Dictionary<int, ConsoleColor> colors) {
            engine = new Engine(this);
            engine.Game(width, height, colors);

            return engine;
        }


        /// <summary> First frame. </summary>
        public abstract void Start ();

        /// <summary> Update frame. </summary>
        public abstract void Update ();

    }
}
