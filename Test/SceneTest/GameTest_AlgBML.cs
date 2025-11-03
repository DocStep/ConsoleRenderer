using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    public class GameTest_AlgBML : Game {
        public GameTest_AlgBML (int height, int width, Dictionary<int, ConsoleColor> colors) : 
            base(height, width, colors) {
            Engine.fpsMax = 10000;
            this.height = height;
            this.width = width;

            arr = new int[height, width];
            arrBuffer = new int[height, width];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    arr[y, x] = new Random().NextDouble() < p ? (new Random().Next(2) == 0 ? 1 : 2) : 0;
            Array.Copy(arr, arrBuffer, arr.Length);
        }


        int[,] arr, arrBuffer;
        float p = 0.5f;
        float q = 0.05f;
        int height;
        int width;


        /*public override void Init () {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    arr[y, x] = new Random().NextDouble() < p ? (new Random().Next(2) == 0 ? 1 : 2) : 0;
            Array.Copy(arr, arrBuffer, arr.Length);
        }*/

        public override void FixedUpdate () {
            Lib.ArrayFill(arr, 0);
            Type1();
            Type2();
            Swap();
            //if (arr.Length != arrBuffer.Length) arrBuffer = new int[arr.GetLength(0), arr.GetLength(1)];
            Array.Copy(arr, arrBuffer, arr.Length);

            Lib.ArrayToDoubled(arr, Renderer.arrCellColor);
        }




        void Type1 () {
            for (int i = 0; i < height; i++) {
                for (int f = 0; f < width-1; f++) {
                    if (arrBuffer[i, f] == 1) {
                        if (arrBuffer[i, f+1] == 0) {
                            arr[i, f] = 0;
                            arr[i, f+1] = 1;
                        } else {
                            arr[i, f] = arrBuffer[i, f];
                        }
                    }
                }
                if (arrBuffer[i, width-1] == 1) {
                    if (arrBuffer[i, 0] == 0) {
                        arr[i, width-1] = 0;
                        arr[i, 0] = 1;
                    } else {
                        arr[i, width-1] = arrBuffer[i, width-1];
                    }
                }
            }
            Complete1();
        }
        void Complete1 () {
            for (int i = 0; i < height; i++)
                for (int f = 0; f < width; f++)
                    if (arr[i, f] == 1) arrBuffer[i, f] = arr[i, f];
        }
        void Type2 () {
            for (int f = 0; f < width; f++) {
                for (int i = 0; i < height-1; i++) {
                    if (arrBuffer[i, f] == 2) {
                        if (arrBuffer[i+1, f] == 0 && arr[i+1, f] == 0) {
                            arr[i+1, f] = arrBuffer[i, f];
                        }
                        else {
                            arr[i, f] = arrBuffer[i, f];
                        }
                    }
                }
                if (arrBuffer[height-1, f] == 2) {
                    if (arrBuffer[0, f] == 0 && arr[0, f] == 0) {
                        arr[0, f] = arrBuffer[height-1, f];
                    }
                    else {
                        arr[height-1, f] = arrBuffer[height-1, f];
                    }
                }
            }
        }
        void Swap () {
            for (int i = 0; i < height; i++)
                for (int f = 0; f < width; f++)
                    switch (arr[i, f]) {
                        case 1:
                            if (new Random().NextDouble() < q) arr[i, f] = 2;
                            break;
                        case 2:
                            if (new Random().NextDouble() < q) arr[i, f] = 1;
                            break;
                    }
        }

    }
}
