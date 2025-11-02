using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    public class GameTest_Glitch : Game {
        public GameTest_Glitch (int height, int width, Dictionary<int, ConsoleColor> colors) : 
            base(height, width, colors) {
            Engine.fpsMax = 10000;
        }


        Random rand = new Random();


        public override void Init () {
            for (int i = 0; i < 10; i++) {
                Engine.Renderer.Write("01..010001101......", 
                    rand.Next(Engine.Renderer.height), rand.Next(Engine.Renderer.width), 2, 1, false);
            }
        }

        public override void Update () {
            for (int i = 0; i < 10; i++) {
                Engine.Renderer.Write("01..010001101......", 
                    rand.Next(Engine.Renderer.height), rand.Next(Engine.Renderer.width)+1, 2, 1, false);
                Engine.Renderer.Write("01..010001101......", 
                    rand.Next(Engine.Renderer.height), rand.Next(Engine.Renderer.width)+1, 1, 3, false);
            }
        }

    }
}
