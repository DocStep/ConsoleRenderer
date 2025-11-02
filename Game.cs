using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleRenderer {
    public abstract class Game : ConsoleGame {

        public Game (int height, int width, Dictionary<int, ConsoleColor> colors) {
            Engine.instance.state = EngineStates.Game;
            Engine.Renderer = new Renderer(height, 2*width, colors);
            Engine.Renderer.videoFrameBuffer = new Pixel[height, width];
            Engine.Renderer.videoFrameBufferGs = new Pixel[height, width];
        }



        



        /*public override void Keys () {
            if (Input.GetKeyDown('Q')) {
                //engine.Menu();
            }
        }*/

    }
}
