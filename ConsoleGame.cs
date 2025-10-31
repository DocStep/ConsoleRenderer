using System;
using System.Collections.Generic;


#pragma warning disable CA1416
namespace ConsoleRenderer {
    public abstract class ConsoleGame {

        public Engine engine;
        //public Renderer renderer;
        //public RendererDebugger debugger;


        public Engine Menu () {
            engine = new Engine(this);
            engine.Menu();

            return engine;
        }

        public Engine Video (string videoPath, int pixelsPerCell, Dictionary<int, ConsoleColor> colors = null, bool useAscii = false) {
            engine = new Engine(this);
            engine.Video(videoPath, pixelsPerCell, colors != null ? colors : DefaultValues.Colors4, useAscii);

            return engine;
        }

        public Engine VideoFromText (string textPath, int height, int width, int fps) {
            engine = new Engine(this);
            engine.VideoFromText(textPath, height, width, fps);

            return engine;
        }

        public Engine CreateGame (int height, int width, Dictionary<int, ConsoleColor> colors) {
            engine = new Engine(this);
            engine.Game(height, width, colors);

            return engine;
        }


        /// <summary> First frame. </summary>
        public abstract void Start ();

        /// <summary> Update frame. </summary>
        public abstract void Update ();

    }
}
