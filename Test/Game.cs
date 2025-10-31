using System;
using System.Diagnostics;
using System.Drawing;
using ConsoleRenderer;


namespace Test {
    class MyGame : ConsoleGame {
        static void Main (string[] args) {
            //Console.WriteLine("video");

            //Console.WriteLine(10);
            //Environment.Exit(0);
            //Console.WriteLine(11);
            game = new MyGame();
            //game.engine.EngineStart(game.Start, game.Update, null, null);
        }
        static MyGame game;
        

        public MyGame () {
            //Console.ReadKey();        
            //Console.WriteLine("video");

            //engine = Menu();

            engine = Video(@"G:/temp/apple.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);
            //engine.fpsMax = 10;

            //engine = Video(@"G:/temp/apple.mp4", 15, false);
            //engine.renderer.colors = new Dictionary<int, ConsoleColor>() {
            //        //{ -1, ConsoleColor.Black },
            //        { 1, ConsoleColor.Red },
            //        { 2, ConsoleColor.White },
            //    };

            //engine = VideoFromText(@"G:/temp/DG2HeroAnimation.txt", 24, 60, 24);
            /*
                        engine = new Engine(this);
                        engine.game = new ConsoleRenderer.Game(engine, 60, 30, new Dictionary<int, ConsoleColor>() {
                                { -1, ConsoleColor.DarkGray },
                                { 0, ConsoleColor.Black },
                                { 1, ConsoleColor.Cyan },
                                { 2, ConsoleColor.Magenta },
                                { 3, ConsoleColor.Green },
                                { 4, ConsoleColor.DarkYellow },
                            });
            */

/*
            engine = CreateGame(30, 30, new Dictionary<int, ConsoleColor>() {
                    { -1, ConsoleColor.DarkGray },
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                    { 4, ConsoleColor.DarkYellow },
                });
*/
        }



        Random r = new Random();
        AlgBML alg = new AlgBML();
        //AlgCircle alg = new AlgCircle();
        //AlgRectangle alg = new AlgRectangle();
        public override void Start () {
            engine.fpsMax = 1000;
            //renderer.debugger.debug = true;

            engine.renderer.arrCellColor = Simple.ArrayRectangulate(alg.Start(Simple.ArraySquare(engine.renderer.arrCellColor, true)));

            //for (int i = 0; i < 10; i++) {
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width), 2, 1, false);
            //}
        }

        public override void Update () {
            engine.renderer.arrCellColor = Simple.ArrayRectangulate(alg.Update());

            //for (int i = 0; i < 10; i++) {
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width)+1, 2, 1, false);
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width)+1, 1, 3, false);
            //}
        }

    }
}