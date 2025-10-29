using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleRenderer {
    public class Simple {

        public static int[,] ArraySquare (int[,] array, bool pickFirstSymbol) {
            int[,] newArray = new int[array.GetLength(0), array.GetLength(1)/2];
            for (int top = 0; top < newArray.GetLength(0); top++)
                for (int left = 0; left < newArray.GetLength(1); left++) {
                    if (pickFirstSymbol) newArray[top, left] = array[top, 2*left];
                    else newArray[top, left] = array[top, 2*left+1];
                }
            return newArray;
        }
        public static int[,] ArrayRectangulate (int[,] array) {
            int[,] newArray = new int[array.GetLength(0), 2*array.GetLength(1)];
            for (int top = 0; top < array.GetLength(0); top++)
                for (int left = 0; left < array.GetLength(1); left++) {
                    newArray[top, 2*left] = array[top, left];
                    newArray[top, 2*left+1] = array[top, left];
                }
            return newArray;
        }

        public static Dictionary<char, char> cellChars = new Dictionary<char, char>() {
            { '{', '}' },
            { '[', ']' },
            { '(', ')' },
            { 'f', 't' },
            { '<', '>' },
        };
        public static char[,] ArrayValuesRectangulate (char[,] array) {
            for (int top = 0; top < array.GetLength(0); top++)
                for (int left = 0; left < array.GetLength(1); left += 2) {
                    if (cellChars.ContainsKey(array[top, 2*left])) {
                        //array[top, 2*left] = array[top, left];
                        cellChars.TryGetValue(array[top, 2*left], out array[top, 2*left+1]);
                    } else {
                        array[top, 2*left+1] = array[top, left];
                    }
                }
            return array;
        }


        public static float[,] ArrayFill (float[,] array, int value) {
            for (int top = 0; top < array.GetLength(0); top++)
                for (int left = 0; left < array.GetLength(1); left++)
                    array[top, left] = value;
            return array;
        }
        public static int[,] ArrayFill (int[,] array, int value) {
            for (int top = 0; top < array.GetLength(0); top++) 
                for (int left = 0; left < array.GetLength(1); left++) 
                    array[top, left] = value;
            return array;
        }
        public static string[,] ArrayFill (string[,] array, string value) {
            for (int top = 0; top < array.GetLength(0); top++) 
                for (int left = 0; left < array.GetLength(1); left++) 
                    array[top, left] = value;
            return array;
        }
        public static char[,] ArrayFill (char[,] array, char value) {
            for (int top = 0; top < array.GetLength(0); top++)
                for (int left = 0; left < array.GetLength(1); left++)
                    array[top, left] = value;
            return array;
        }

        public static int[,] ArrayFlip (int[,] arr) {
            for (int left = 0; left < arr.GetLength(1); left++) 
                for (int top = 0; top < arr.GetLength(0); top++) {
                    int t = arr[arr.GetLength(0)-1-top, left];
                    arr[arr.GetLength(0)-1-top, left] = arr[top, left];
                    arr[top, left] = t;
                }
            return arr;
        }


        public static void Fill (ConsoleColor color) {
            Console.Clear();
            Console.BackgroundColor = color;
            for (int y = 0; y < Console.BufferHeight; y++) {
                Console.SetCursorPosition(0, y);
                string s = "";
                for (int x = 0; x < Console.BufferWidth; x++) s += "  ";
                Console.Write(s);
            }
        }


        public static void WriteArray (int[,] arr) {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            for (int top = 0; top < arr.GetLength(0); top++) 
                for (int left = 0; left < arr.GetLength(1); left++) 
                    Console.Write(arr[top, left]);
        }

        public static void WriteArray (string[,] arr) {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            for (int top = 0; top < arr.GetLength(0); top++) 
                for (int left = 0; left < arr.GetLength(1); left++) 
                    Console.Write(arr[top, left]);
        }

        public static void WriteArrayColor (int[,] arr, Dictionary<int, ConsoleColor> colors) {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            for (int top = 0; top < arr.GetLength(0); top++)
                for (int left = 0; left < arr.GetLength(1); left++) {
                    Console.BackgroundColor = colors.First(x => x.Key == arr[top, left]).Value;
                    Console.Write(" ");
                }
        }

    }
}
