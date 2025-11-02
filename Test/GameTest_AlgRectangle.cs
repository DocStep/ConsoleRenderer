using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    public class GameTest_AlgRectangle : Game {

        public GameTest_AlgRectangle (int height, int width, Dictionary<int, ConsoleColor> colors) : 
            base(height, width, colors) {
            arr = new int[height, width];
            this.height = height;
            this.width = width;
            Engine.fpsMax = 10000;
        }


        int[,] arr;
        float size = 0.5f;
        float moveR = 0.5f;
        int iter = -1;
        int height;
        int width;


        public override void Init () {
            for (int y = 0; y < arr.GetLength(0); y++) {
                for (int x = 0; x < arr.GetLength(1); x++) {
                    arr[y, x] = 0;
                    if ((1-size)*0.5f*width < x && x < (1+size)*0.5f*width &&
                        (1-size)*0.5f*height < y && y < (1+size)*0.5f*height) {
                        arr[y, x] = 1;
                    }
                }
            }
        }

        public override void Update () {
            iter++;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    arr[y, x] = 0;
                    int marginX = (int)(Math.Cos(iter/20f)*moveR*width);
                    int marginY = (int)(Math.Sin(iter/20f)*moveR*height);
                    if ((1-size)*0.5f*height < y+0.5f*marginY && y+0.5f*marginY < (1+size)*0.5f*height &&
                        (1-size)*0.5f*width < x+0.5f*marginX && x+0.5f*marginX < (1+size)*0.5f*width) {
                        arr[y, x] = 1;
                    }
                }
            }

            lib.ArrayToDoubled(arr, Engine.Renderer.arrCellColor);
        }

    }
}
