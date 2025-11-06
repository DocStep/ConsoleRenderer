using System;
using System.Drawing;


namespace ConsoleRenderer;

public abstract class Game : Scene {
    public Game (int height, int width, Dictionary<int, ConsoleColor> colors) {
        Engine.state = EngineStates.Game;
        Engine.fpsMax = 100;
        Renderer.Init(height, 2*width, colors);
        Renderer.cellFrameBuffer = new Pixel[height, width];
        Renderer.cellFrameBufferGs = new Pixel[height, width];
    }


    /*public override void Keys () {
        if (Input.GetKeyDown('Q')) {
            //engine.Menu();
        }
    }*/

}
