using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    class AlgRectangle {

        int[,] arr;
        float size = 0.5f;
        float moveR = 0.5f;
        int iter = -1;

        public int[,] Start (int[,] arr) {
            this.arr = arr;

            for (int y = 0; y < arr.GetLength(0); y++) {
                for (int x = 0; x < arr.GetLength(1); x++) {
                    arr[y, x] = 0;
                    if ((1-size)*arr.GetLength(1)/2 < x && x < (1+size)*arr.GetLength(1)/2 &&
                        (1-size)*arr.GetLength(0)/2 < y && y < (1+size)*arr.GetLength(0)/2) {
                        arr[y, x] = 1;
                    }
                }
            }
            return arr;
        }
        public int[,] Start (int[,] arr, float size, float moveR) {
            this.size = size;
            this.moveR = moveR;
            return Start(arr);
        }


        public int[,] Update () {
            iter++;
            for (int y = 0; y < arr.GetLength(0); y++) {
                for (int x = 0; x < arr.GetLength(1); x++) {
                    arr[y, x] = 0;
                    int sizeX = arr.GetLength(1);
                    int sizeY = arr.GetLength(0);
                    int marginX = (int)(sizeX*Math.Cos(iter/20f)*moveR);
                    int marginY = (int)(sizeY*Math.Sin(iter/20f)*moveR);
                    if ((1-size)*arr.GetLength(1)/2 < x+marginX/2 && x+marginX/2 < (1+size)*arr.GetLength(1)/2 &&
                        (1-size)*arr.GetLength(0)/2 < y+marginY/2 && y+marginY/2 < (1+size)*arr.GetLength(0)/2) {
                        arr[y, x] = 1;
                    }

                    /*if ((1-r)*arr.GetLength(1)/2 < x+marginX/2 && x < (1+r)*arr.GetLength(1)/2+marginX/2 &&
                        (1-r)*arr.GetLength(0)/2 < y+marginY/2 && y < (1+r)*arr.GetLength(0)/2+marginY/2) {
                        arr[y, x] = 1;
                    }*/
                }
            }
            return arr;
        }

    }
}
