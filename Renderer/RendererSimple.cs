using ConsoleRenderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleRenderer;

public static class RendererSimple {

    public static int colorCell = 0;
    public static ConsoleColor c_colorCell = ConsoleColor.Black;
    public static int colorText = 0;
    public static ConsoleColor c_colorText = ConsoleColor.White;


    public static void WindowInit (int height, int width) {
        RendererSimple.Wrapping(false);

        //Renderer.heightWindow = height;
        //Console.CursorVisible = false;
        Console.SetWindowSize(width, height);
        Console.SetBufferSize(width, height);
        Console.SetWindowSize(width, height);
        //Console.SetWindowSize(width, Renderer.heightWindow);
    }
    public static void Wrapping (bool wrapping) {
        Renderer.lineWrapping = wrapping;
        if (Renderer.lineWrapping) {
            IntPtr handle = Lib_dlls.GetStdHandle(Lib_dlls.STD_OUTPUT_HANDLE);
            Lib_dlls.GetConsoleMode(handle, out uint mode);
            uint modeNoWrap = mode & ~Lib_dlls.ENABLE_WRAP_AT_EOL_OUTPUT;
            Lib_dlls.SetConsoleMode(handle, modeNoWrap);
        }

        //Renderer.heightWindow = Renderer.height;
        //Console.SetWindowSize(Renderer.width, Renderer.heightWindow);
    }



    public static Dictionary<int, int> colorsCounts = new Dictionary<int, int>();
    public static int i_GetCellsMostColor () {
        colorsCounts.Clear();
        colorsCounts = Renderer.colors.ToDictionary(pair => pair.Key, pair => 0);
        //foreach (var color in Renderer.colors) colorsCounts.Add(color.Key, 0);
        for (int top = 0; top < Renderer.height; top++)
            for (int left = 0; left < Renderer.width; left++)
                colorsCounts[Renderer.arrCellColor[top, left]]++;

        //KeyValuePair<int, int> mostColor = colorsCounts.Aggregate((l, r) => r.Value < l.Value ? l : r);
        int i_mostColor = 0;
        int i_mostColorCount = 0;
        foreach (var color in colorsCounts) {
            if (i_mostColorCount < color.Value) {
                i_mostColorCount = color.Value;
                i_mostColor = color.Key;
            }
        }
        //Console.Title = $"{i_mostColor}";

        return i_mostColor;
    }



    /// Set Color Sell
    public static void SetColorCell (int colorCell) {
        if (RendererSimple.colorCell != colorCell) {
            RendererSimple.colorCell = colorCell;
            Console.BackgroundColor = Renderer.colors.First(x => x.Key == colorCell).Value;
            RendererDebugger.SetColorCell();
        }
    }
    public static void SetColorCell (ConsoleColor c_colorCell) {
        int i_color = Renderer.colors.First(x => x.Value == c_colorCell).Key;
        if (RendererSimple.colorCell != i_color) {
            RendererSimple.colorCell = i_color;
            RendererSimple.c_colorCell = c_colorCell;
            Console.BackgroundColor = c_colorCell;
            RendererDebugger.SetColorCell();
        }
    }

    /// Set Color Text
    public static void SetColorText (int colorText) {
        if (RendererSimple.colorText != colorText) {
            RendererSimple.colorText = colorText;
            Console.ForegroundColor = Renderer.colors.First(x => x.Key == colorText).Value;
            //Console.ForegroundColor = Renderer.colors.First(x => x.Key == colorText).Value;
            RendererDebugger.SetColorText();
        }
    }
    public static void SetColorText (ConsoleColor c_colorText) {
        int i_color = Renderer.colors.First(x => x.Value == c_colorCell).Key;
        if (RendererSimple.c_colorText != c_colorText) {
            RendererSimple.c_colorText = c_colorText;
            RendererSimple.colorText = i_color;
            Console.ForegroundColor = c_colorText;
            RendererDebugger.SetColorText();
        }
    }

    /// Set Cursor
    public static void SetCursor (int top, int left) {
        //if (top != Console.GetCursorPosition().Top || left != Console.GetCursorPosition().Left) {
        if (top != Console.CursorTop || left != Console.CursorLeft) {
            //cursorTop = Console.GetCursorPosition().Top;
            //cursorLeft = Console.GetCursorPosition().Left;
            //Console.SetWindowSize(width, height);
            //if (left < Console.WindowWidth && left < Console.WindowHeight) 
            Console.SetCursorPosition(left, top);
            RendererDebugger.SetCursor();
        }
    }
    //static int cursorTop = 0;
    //static int cursorLeft = 0;
    public static void Write (string s) {
        Console.Write(s);
        RendererDebugger.Write();
    }


    public static void Fill (ConsoleColor color) {
        RendererSimple.SetColorCell(color);
        string text = new string(' ', Console.BufferHeight*Console.BufferWidth);
        RendererSimple.SetCursor(0, 0);
        RendererSimple.Write(text);
    }
    public static void FillWrapping (ConsoleColor color) {
        RendererSimple.SetColorCell(color);
        string text = new string(' ', Renderer.width);
        for (int top = 0; top < Renderer.height; top++) {
            RendererSimple.SetCursor(top, 0);
            RendererSimple.Write(text);
        }
        RendererSimple.Write(text);
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
