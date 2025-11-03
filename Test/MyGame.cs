using System;
using System.Diagnostics;
using System.Drawing;
using ConsoleRenderer;


namespace Test {
    class MyGame {
        static void Main (string[] args) {
            Engine.Init();
            SceneManager.Current = new Video(@"G:/temp/apple.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);
            Engine.Start();
        }
    }
}