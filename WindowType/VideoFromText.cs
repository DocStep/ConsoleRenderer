using System;


namespace ConsoleRenderer {
    public class VideoFromText : Scene {
        public VideoFromText (string path, int height, int width, int fps) {
            Engine.state = EngineStates.VideoFromText;
            Renderer.Init(height, width);
            Renderer.title = Path.GetFileName(path);
            Renderer.forceAscii = true;
            Engine.fpsMax = fps;
            sr = new StreamReader(path);
        }


        StreamReader sr;
        string text = "";


        public override void FixedUpdate () {
            text = "";
            for (int i = 0; i < Renderer.height; i++) {
                text += sr.ReadLine() + (i < Renderer.height-1 ? "\n" : "");
                //Renderer.Write(sr.ReadLine(), i, 0, 1, 0, true);
            }

            if (text == "") Engine.engineWork = false;

            Renderer.textAscii = text;
        }


        public override void Inputs () {
            if (Input.GetKeyDown('Q')) {
                //engine.Menu();
            }

        }

    }
}
