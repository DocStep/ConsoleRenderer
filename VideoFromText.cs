using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConsoleRenderer {
    public class VideoFromText : ConsoleGame {

        StreamReader sr;
        string text = "";

        public VideoFromText (Engine engine, string path, int height, int width, int fps) {
            Engine.instance.state = EngineStates.VideoFromText;
            Engine.Renderer = new Renderer(height, width);
            Engine.Renderer.title = Path.GetFileName(path);
            Engine.Renderer.forceAscii = true;
            Engine.fpsMax = fps;
            sr = new StreamReader(path);
        }



        public void Keys () {
            if (Input.GetKeyDown('Q')) {
                //engine.Menu();
            }

        }


        public override void Update () {
            text = "";
            for (int i = 0; i < Engine.Renderer.height; i++) {
                text += sr.ReadLine() + (i < Engine.Renderer.height-1 ? "\n" : "");
                //renderer.Write(sr.ReadLine(), i, 0, 1, 0, true);
            }

            if (text == "") Engine.instance.engineWork = false;

            Engine.Renderer.textAscii = text;
        }


    }
}
