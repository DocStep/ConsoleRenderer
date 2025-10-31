using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConsoleRenderer {
    public class VideoFromText {

        public Engine engine;
        //public Renderer renderer;

        StreamReader sr;
        string text = "";

        public VideoFromText (Engine engine, string path, int height, int width, int fps) {
            engine.state = States.videoFromText;
            engine.renderer = new Renderer(height, width);
            engine.renderer.title = Path.GetFileName(path);
            engine.renderer.forceAscii = true;
            engine.fpsMax = fps;
            sr = new StreamReader(path);

            this.engine = engine;
            //renderer = engine.renderer;
        }



        public void Keys () {
            if (Input.GetKeyDown('Q')) {
                engine.Menu();
            }

        }
        public void Start () {

        }
        public void Update () {
            text = "";
            for (int i = 0; i < engine.renderer.height; i++) {
                text += sr.ReadLine() + (i < engine.renderer.height-1 ? "\n" : "");
                //renderer.Write(sr.ReadLine(), i, 0, 1, 0, true);
            }

            if (text == "") engine.engineWork = false;

            engine.renderer.textAscii = text;
        }
        public void UpdateSkip () {

        }
        public void Exit () {

        }

    }
}
