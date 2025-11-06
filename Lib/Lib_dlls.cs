using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;


namespace ConsoleRenderer;

public static class Lib_dlls {
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole ();

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle (int nStdHandle);
    [DllImport("kernel32.dll")]
    public static extern bool GetConsoleMode (IntPtr hConsoleHandle, out uint lpMode);
    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleMode (IntPtr hConsoleHandle, uint dwMode);
    [DllImport("kernel32.dll")]
    public static extern bool GetConsoleScreenBufferInfo (IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

    public const int STD_OUTPUT_HANDLE = -11;
    //public const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;
    public const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD { public short X, Y; }
    [StructLayout(LayoutKind.Sequential)]
    public struct SMALL_RECT { public short Left, Top, Right, Bottom; }
    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO {
        public COORD Size;
        public COORD CursorPosition;
        public short Attributes;
        public SMALL_RECT Window;
        public COORD MaximumWindowSize;
    }


    [DllImport("kernel32")]
    public static extern bool SetConsoleIcon (IntPtr hIcon);

    /*public static bool SetConsoleIcon (Icon icon) {
        return SetConsoleIcon(icon.Handle);
    }*/

    [DllImport("kernel32")]
    private extern static bool SetConsoleFont (IntPtr hOutput, uint index);

    private enum StdHandle {
        OutputHandle = -11
    }

    [DllImport("kernel32")]
    private static extern IntPtr GetStdHandle (StdHandle index);

    public static bool SetConsoleFont (uint index) {
        return SetConsoleFont(GetStdHandle(StdHandle.OutputHandle), index);
    }

    [DllImport("kernel32")]
    private static extern bool GetConsoleFontInfo (IntPtr hOutput, [MarshalAs(UnmanagedType.Bool)] bool bMaximize,
        uint count, [MarshalAs(UnmanagedType.LPArray), Out] ConsoleFont[] fonts);

    [DllImport("kernel32")]
    private static extern uint GetNumberOfConsoleFonts ();

    public static uint ConsoleFontsCount {
        get {
            return GetNumberOfConsoleFonts();
        }
    }

    public static ConsoleFont[] ConsoleFonts {
        get {
            ConsoleFont[] fonts = new ConsoleFont[GetNumberOfConsoleFonts()];
            if (fonts.Length > 0)
                GetConsoleFontInfo(GetStdHandle(StdHandle.OutputHandle), false, (uint)fonts.Length, fonts);
            return fonts;
        }
    }

}


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ConsoleFont {
    public uint Index;
    public short SizeX, SizeY;
}
