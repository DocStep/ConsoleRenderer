namespace ConsoleRenderer;

public class Lib {

    public static int[,] ArrayToHalf (int[,] array, int[,] halfed, bool pickFirstSymbol) {
        int length0 = array.GetLength(0);
        int length1 = array.GetLength(1);
        int length1New = length1/2;
        for (int top = 0; top < length0; top++)
            for (int left = 0; left < length1New; left++) {
                if (pickFirstSymbol) halfed[top, left] = array[top, 2*left];
                else halfed[top, left] = array[top, 2*left+1];
            }
        return halfed;
    }
    public static int[,] ArrayToDoubled (int[,] array, int[,] doubled) {
        int length0 = array.GetLength(0);
        int length1 = array.GetLength(1);
        int length1New = 2*length1;
        for (int top = 0; top < length0; top++)
            for (int left = 0; left < length1; left++) {
                doubled[top, 2*left] = array[top, left];
                doubled[top, 2*left+1] = array[top, left];
            }
        return doubled;
    }

    public static Dictionary<char, char> cellChars = new Dictionary<char, char>() {
        { '{', '}' },
        { '[', ']' },
        { '(', ')' },
        { 'f', 't' },
        { '<', '>' },
    };
    public static char[,] ArrayValuesRectangulate (char[,] array) {
        int length0 = array.GetLength(0);
        int length1 = array.GetLength(1);
        for (int top = 0; top < length0; top++)
            for (int left = 0; left < length1; left += 2) {
                if (cellChars.ContainsKey(array[top, 2*left])) {
                    //array[top, 2*left] = array[top, left];
                    cellChars.TryGetValue(array[top, 2*left], out array[top, 2*left+1]);
                } else {
                    array[top, 2*left+1] = array[top, left];
                }
            }
        return array;
    }


    public static T[,] ArrayFill<T> (T[,] array, T value) {
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
        Console.ForegroundColor = DefaultValues.c_Text;
        Console.BackgroundColor = DefaultValues.c_Cell;
        for (int top = 0; top < arr.GetLength(0); top++) 
            for (int left = 0; left < arr.GetLength(1); left++) 
                Console.Write(arr[top, left]);
    }

    public static void WriteArray (string[,] arr) {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = DefaultValues.c_Text;
        Console.BackgroundColor = DefaultValues.c_Cell;
        for (int top = 0; top < arr.GetLength(0); top++) 
            for (int left = 0; left < arr.GetLength(1); left++) 
                Console.Write(arr[top, left]);
    }

    public static void WriteArrayColor (int[,] arr, Dictionary<int, ConsoleColor> colors) {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = DefaultValues.c_Text;
        Console.BackgroundColor = DefaultValues.c_Cell;
        for (int top = 0; top < arr.GetLength(0); top++)
            for (int left = 0; left < arr.GetLength(1); left++) {
                Console.BackgroundColor = colors.First(x => x.Key == arr[top, left]).Value;
                Console.Write(" ");
            }
    }

}
