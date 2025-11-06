using System;
using System.Collections.Generic;
using System.Text;
using ConsoleRenderer;
using System.Drawing;


namespace Test;

public class GameTest_Line : Game {
    public GameTest_Line (int height, int width, Dictionary<int, ConsoleColor> colors) : 
        base(height, width, colors) {
        Engine.fpsMax = 10f;

        str = new string('a', 2*width);
    }

    string str;

    public override void FixedUpdate () {
        //Renderer.WriteText(str, 0, 0, 1, 0);
        Console.Write(str);
        // Caret is invisible, so it won’t appear
        //Renderer("Done! Cursor hidden.");
    }


}
