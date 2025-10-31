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

            engine = Video(@"G:/temp/apple.mp4", 15, false);
            //engine.fpsMax = 10;

            //engine = Video(@"G:/temp/apple.mp4", 15, false);
            //engine.renderer.colors = new Dictionary<int, ConsoleColor>() {
            //        //{ -1, ConsoleColor.Black },
            //        { 1, ConsoleColor.Red },
            //        { 2, ConsoleColor.White },
            //    };

            //engine = VideoFromText(@"D:/DG2HeroAnimation.txt", 60, 24, 24);
            /*engine = new Engine(this);
            engine.game = new ConsoleRenderer.Game(engine, 60, 30, new Dictionary<int, ConsoleColor>() {
                    { -1, ConsoleColor.DarkGray },
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                    { 4, ConsoleColor.DarkYellow },
                });*/
            //renderer = game.engine.renderer;


            /*engine = CreateGame(60, 30, new Dictionary<int, ConsoleColor>() {
                    { -1, ConsoleColor.DarkGray },
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                    { 4, ConsoleColor.DarkYellow },

                });*/
            //renderer = engine.renderer;

        }



        Random r = new Random();
        AlgBML alg = new AlgBML();
        //AlgCircle alg = new AlgCircle();
        //AlgRectangle alg = new AlgRectangle();
        public override void Start () {
            renderer = engine.renderer;
            engine.fpsMax = 1000;
            //renderer.debugger.debug = true;

            renderer.arrCellColor = Simple.ArrayRectangulate(alg.Start(Simple.ArraySquare(renderer.arrCellColor, true)));

            //for (int i = 0; i < 10; i++) {
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width), 2, 1, false);
            //}
        }

        public override void Update () {
            renderer.arrCellColor = Simple.ArrayRectangulate(alg.Update());

            //for (int i = 0; i < 10; i++) {
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width)+1, 2, 1, false);
            //    renderer.Write("01..010001101......", r.Next(renderer.height), r.Next(renderer.width)+1, 1, 3, false);
            //}
        }

    }
}