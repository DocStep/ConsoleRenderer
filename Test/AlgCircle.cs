using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    class AlgCircle {

        int[,] arr;
        float r = 0.5f;
        float moveR = 0.25f;
        int iter = -1;

        public int[,] Start (int[,] arr) {
            this.arr = arr;

            for (int y = 0; y < arr.GetLength(0); y++) {
                for (int x = 0; x < arr.GetLength(1); x++) {
                    arr[y, x] = 0;
                    if (Math.Pow(x-arr.GetLength(1)/2, 2) + Math.Pow(y-arr.GetLength(0)/2, 2) <
                        (int)Math.Pow((arr.GetLength(0) < arr.GetLength(1) ? arr.GetLength(0) : arr.GetLength(1))/2*this.r, 2))
                        arr[y, x] = 1;
                }
            }
            return arr;
        }
        public int[,] Start (int[,] arr, float r, float moveR) {
            this.r = r;
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
                    int marginX = (int)(sizeX*Math.Cos(iter/10f)*moveR);
                    int marginY = (int)(sizeY*Math.Sin(iter/10f)*moveR);

                    if (Math.Pow(x-sizeX/2+marginX, 2) + Math.Pow(y-sizeY/2+marginY, 2) <=
                        (int)Math.Pow((arr.GetLength(0) < arr.GetLength(1) ? arr.GetLength(0) : arr.GetLength(1))/2*r, 2))
                        arr[y, x] = 1;
                }
            }
            return arr;
        }

    }
}
