using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    class AlgHalf {
        int[,] arr;
        float r, moveR;

        public int[,] Start (int[,] arr, int color, float length) {
            for (int y = 0; y < arr.GetLength(0); y++) 
                for (int x = 0; x < arr.GetLength(1); x++) 
                    arr[y, x] = x < arr.GetLength(1)*length ? color : 0;
            this.arr = arr;
            return arr;
        }

        public int[,] Update (int[,] arr, int color, float length) {
            for (int y = 0; y < arr.GetLength(0); y++) 
                for (int x = 0; x < arr.GetLength(1); x++) 
                    arr[y, x] = x < arr.GetLength(1)*length ? color : 0;
            this.arr = arr;
            return arr;
        }
    }
}
