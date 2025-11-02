using System;
using System.Diagnostics;
using System.Drawing;
using ConsoleRenderer;


namespace Test {
    class MyGame {
        static void Main (string[] args) {
            Engine engine = new Engine();

            Engine.ConsoleGame = new Video(@"G:/temp/apple.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);

            Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>() {
                    { -1, ConsoleColor.DarkGray },
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                    { 4, ConsoleColor.DarkYellow },
                };
            //Engine.ConsoleGame = new GameTest_AlgBML(30, 30, colors: colors);
            //Engine.ConsoleGame = new GameTest_AlgCircle(30, 30, colors: colors);
            //Engine.ConsoleGame = new GameTest_AlgRectangle(30, 30, colors: colors);
            /*Engine.ConsoleGame = new GameTest_Glitch(30, 30, colors: colors = new Dictionary<int, ConsoleColor>() {
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                });*/
            //Engine.fpsMax = 10;

            //Engine.ConsoleGame = new Menu("Menu", true);

            //Engine.ConsoleGame = new VideoFromText(@"G:/temp/DG2HeroAnimation.txt", 24, 60, 24);

            //Engine.ConsoleGame.Init();

            Engine.ConsoleGame.Init();
            engine.Start();
        }
        //static Engine engine;
    }
}