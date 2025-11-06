using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Test;

//#pragma warning disable CA1416
//#pragma warning disable CS8618
public class GameTest_Glitch : Game {
    //public GameTest_Glitch (int height, int width, Dictionary<int, ConsoleColor> colors) : 
    public GameTest_Glitch (int height, int width, Dictionary<int, ConsoleColor> colors) : 
        base(height, width, colors) {
        Engine.fpsMax = 20f;
    }


    Random rand = new Random();


    public override void FixedUpdate () {
        for (int i = 0; i < 10; i++) {
            Renderer.WriteText("01..010001101......", 
                rand.Next(Renderer.height), rand.Next(Renderer.width)+1, 2, 1, false);
            Renderer.WriteText("01..010001101......", 
                rand.Next(Renderer.height), rand.Next(Renderer.width)+1, 1, 3, false);
        }
    }

}
