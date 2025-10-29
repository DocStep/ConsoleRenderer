using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CA1416
#pragma warning disable CS8618
namespace Test {
    public class AlgBML {

        int[,] arr, arrBuffer;
        float p = 0.5f;
        float q = 0.05f;

        public int[,] Start (int[,] arr) {
            this.arr = arr;
            arrBuffer = new int[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
                for (int x = 0; x < arr.GetLength(1); x++)
                    arr[y, x] = new Random().NextDouble() < p ? (new Random().Next(2) == 0 ? 1 : 2) : 0;
            Array.Copy(arr, arrBuffer, arr.Length);

            return arr;
        }
        public int[,] Start (int[,] arr, float p, float q) {
            this.p = p;
            this.q = q;
            return Start(arr);
        }


        public int[,] Update () {
            arr = new int[arr.GetLength(0), arr.GetLength(1)];
            Type1();
            Type2();
            Swap();
            //if (arr.Length != arrBuffer.Length) arrBuffer = new int[arr.GetLength(0), arr.GetLength(1)];
            Array.Copy(arr, arrBuffer, arr.Length);
            return arr;
        }

        void Type1 () {
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int f = 0; f < arr.GetLength(1)-1; f++) {
                    if (arrBuffer[i, f] == 1) {
                        if (arrBuffer[i, f+1] == 0) {
                            arr[i, f] = 0;
                            arr[i, f+1] = 1;
                        }
                        else {
                            arr[i, f] = arrBuffer[i, f];
                        }
                    }
                }
                if (arrBuffer[i, arr.GetLength(1)-1] == 1) {
                    if (arrBuffer[i, 0] == 0) {
                        arr[i, arr.GetLength(1)-1] = 0;
                        arr[i, 0] = 1;
                    }
                    else {
                        arr[i, arr.GetLength(1)-1] = arrBuffer[i, arr.GetLength(1)-1];
                    }
                }
            }
            Complete1();
        }
        void Complete1 () {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int f = 0; f < arr.GetLength(1); f++)
                    if (arr[i, f] == 1) arrBuffer[i, f] = arr[i, f];
        }
        void Type2 () {
            for (int f = 0; f < arr.GetLength(1); f++) {
                for (int i = 0; i < arr.GetLength(0)-1; i++) {
                    if (arrBuffer[i, f] == 2) {
                        if (arrBuffer[i+1, f] == 0 && arr[i+1, f] == 0) {
                            arr[i+1, f] = arrBuffer[i, f];
                        }
                        else {
                            arr[i, f] = arrBuffer[i, f];
                        }
                    }
                }
                if (arrBuffer[arr.GetLength(0)-1, f] == 2) {
                    if (arrBuffer[0, f] == 0 && arr[0, f] == 0) {
                        arr[0, f] = arrBuffer[arr.GetLength(0)-1, f];
                    }
                    else {
                        arr[arr.GetLength(0)-1, f] = arrBuffer[arr.GetLength(0)-1, f];
                    }
                }
            }
        }
        void Swap () {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int f = 0; f < arr.GetLength(1); f++)
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
