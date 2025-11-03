using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    public class GameTest_AlgCircle : Game {

        public GameTest_AlgCircle (int height, int width, Dictionary<int, ConsoleColor> colors) : 
            base(height, width, colors) {
            Engine.fpsMax = 10000;
            this.height = height;
            this.width = width;

            arr = new int[height, width];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    arr[y, x] = 0;
                    if (Math.Pow(x - 0.5f*width, 2) + Math.Pow(y - 0.5f*height, 2) <
                        (int)Math.Pow(0.5f*r*Math.Min(height, width), 2))
                        arr[y, x] = 1;
                }
            }
        }


        int[,] arr;
        float r = 0.5f;
        float moveR = 0.25f;
        int iter = -1;
        int height;
        int width;


        /*public override void Init () {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    arr[y, x] = 0;
                    if (Math.Pow(x - 0.5f*width, 2) + Math.Pow(y - 0.5f*height, 2) <
                        (int)Math.Pow(0.5f*r*Math.Min(height, width), 2))
                        arr[y, x] = 1;
                }
            }
        }*/

        public override void FixedUpdate () {
            iter++;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    arr[y, x] = 0;
                    int marginX = (int)(width*Math.Cos(iter/10f)*moveR);
                    int marginY = (int)(height*Math.Sin(iter/10f)*moveR);

                    if (Math.Pow(x-width/2+marginX, 2) + Math.Pow(y-height/2+marginY, 2) <=
                        (int)Math.Pow(0.5f*r*Math.Min(height, width), 2))
                        arr[y, x] = 1;
                }
            }

            Lib.ArrayToDoubled(arr, Renderer.arrCellColor);
        }

    }
}
