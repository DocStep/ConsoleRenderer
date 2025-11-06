using ConsoleRenderer;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Test;

class MyGame {
    static void Main (string[] args) {

        Engine.Init();

        SceneManager.Current = new Video(@"G:/temp/apple.mp4", pixelsPerCell: 20, colors: DefaultValues.Colors2, useAscii: false);

        //SceneManager.Current = new GameTest_Line(1, 50, DefaultValues.Colors2);

        //SceneManager.Current = new VideoFromText(@"G:/temp/DG2HeroAnimation.txt", height: 24, width: 60, fps: 24);

        //SceneManager.Current = new GameTest_AlgBML(height: 30, width: 30, DefaultValues.Colors4);
        //Renderer.Passer = new PasserFull();
        //Engine.fpsMax = 20;

        //SceneManager.Current = new GameTest_AlgCircle(height: 30, width: 30, DefaultValues.Colors2);
        //SceneManager.Current = new GameTest_AlgRectangle(height: 30, width: 30, DefaultValues.Colors2);
        //SceneManager.Current = new GameTest_Glitch(height: 30, width: 30, DefaultValues.Colors4);

        Renderer.Passer = new PasserColorSort();
        //Renderer.Passer = new PasserColorChangeSets();

        //Engine.fpsMax = 100;
        Engine.Start();

        /*
                IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
                int width = Console.WindowWidth;

                // Remember current cursor row (e.g. top)
                int baseTop = Console.CursorTop;



                // For debug: check final position
                GetConsoleScreenBufferInfo(handle, out var info);
        *//*
                System.Console.ReadKey();*/
    }

    [DllImport("kernel32.dll")] static extern IntPtr GetStdHandle (int nStdHandle);
    [DllImport("kernel32.dll")] static extern bool GetConsoleScreenBufferInfo (IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

    const int STD_OUTPUT_HANDLE = -11;

    [StructLayout(LayoutKind.Sequential)]
    struct COORD { public short X, Y; }
    [StructLayout(LayoutKind.Sequential)]
    struct SMALL_RECT { public short Left, Top, Right, Bottom; }
    [StructLayout(LayoutKind.Sequential)]
    struct CONSOLE_SCREEN_BUFFER_INFO {
        public COORD Size;
        public COORD CursorPosition;
        public short Attributes;
        public SMALL_RECT Window;
        public COORD MaximumWindowSize;
    }

}
