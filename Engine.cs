using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using static ConsoleRenderer.DefaultValues;
using static ConsoleRenderer.Input;


public enum States {
    none,
    menu,
    video,
    videoFromText,
    game,
}

#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {
    public class Engine {

        public ConsoleGame consoleGame;
        public Renderer renderer;
        public Thread mainThread, controlThread;
        public Input input;

        public MenuCanvas menu;
        public Video video;
        public VideoFromText videoFromText;
        public Game game;

        public delegate void Void ();
        public Void de_Start;
        public Void de_Update;
        public Void Start;
        public Void Update;
        public Void UpdateSkip;
        public Void Exit;


        public Engine (ConsoleGame game) {
            consoleGame = game;
            de_Start += game.Start;
            de_Update += game.Update;
            input = new Input();
            threadsStartCount = Process.GetCurrentProcess().Threads.Count;
        }
        public void EngineStart (Void start, Void update, Void updateSkip, Void exit) {
            Start = start;
            Update = update;
            UpdateSkip = updateSkip;
            Exit = exit;

            nextIterTime = DateTime.Now.Ticks + (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
            controlThread = new Thread(ThreadControl);
            controlThread.Start();
            mainThread = new Thread(ThreadEngine);
            mainThread.Start();

            renderer.debugger.threadsCount = Process.GetCurrentProcess().Threads.Count - threadsStartCount;

            // Start dStart
        }


        public States state;
        public bool engineWork;
        public bool appWork;
        public int threadsStartCount;
        public long nextIterTime;
        public int framesQueue = 0;


        public bool isEngineWorking;
        public bool isPassing;
        public bool isSelfEnd;

        public double fpsMax = 60;



        void ThreadEngine () {
            isEngineWorking = true;
            engineWork = true;

            if (Start != null) {
                isPassing = true;
                Start();
                if (state == States.game) de_Start.Invoke();

                renderer.Pass();
                isPassing = false;
            }

            while (engineWork) {
                if (framesQueue > 0) {
                    isPassing = true;

                    // Frame skip
                    int skipped = 0;
                    while (framesQueue > 1 && skipped < 10) {
                        renderer.debugger.framesSkipped++;
                        //skipped++;
                        framesQueue--;

                        if (UpdateSkip != null) UpdateSkip();
                    }

                    // Frame
                    framesQueue--;

                    if (Update != null) Update?.Invoke();
                    if (state == States.game) de_Update?.Invoke();

                    renderer.debugger.framesQueue = framesQueue;
                    renderer.Pass();
                    isPassing = false;
                }
            }

            ExitThreadEngine();
        }
        void ThreadControl () {
            appWork = true;
            nextIterTime = DateTime.Now.Ticks;
            while (appWork) {
                Keys();

                if (DateTime.Now.Ticks >= nextIterTime + (long)(1f/fpsMax*TimeSpan.TicksPerSecond)) {
                    nextIterTime += (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
                    framesQueue++;
                }
            }

            ExitThreadControl();
        }
        void Keys () {
            switch (state) {
                case States.menu:
                    if (menu != null) menu.Keys();
                    break;
                case States.video:
                    if (video != null) video.Keys();
                    break;
                case States.videoFromText:
                    if (videoFromText != null) videoFromText.Keys();
                    break;
                case States.game:
                    if (game != null) game.Keys();
                    break;
            }

        }



        public void Menu () {
            ExitThreadEngine();

            menu = new MenuCanvas(this);
            renderer = menu.engine.renderer;

            EngineStart(menu.Start, menu.Update, menu.UpdateSkip, menu.Exit);
        }
        
        public void Video (string path, int pixelsPerCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
            ExitThreadEngine();

            video = new Video(this, path, pixelsPerCell, DefaultValues.colorsGreys3, useAscii);
            renderer = video.engine.renderer;

            EngineStart(video.Start, video.Update, video.UpdateSkip, video.Exit);
        }

        public void VideoFromText (string path, int width, int height, int fps) {
            ExitThreadEngine();

            videoFromText = new VideoFromText(this, path, width, height, fps);
            renderer = videoFromText.engine.renderer;

            EngineStart(videoFromText.Start, videoFromText.Update, videoFromText.UpdateSkip, videoFromText.Exit);
        }
        
        public void Game (int width, int height, Dictionary<int, ConsoleColor> colors) {
            ExitThreadEngine();

            game = new Game(this, width, height, colors);
            renderer = game.engine.renderer;

            EngineStart(game.Start, game.Update, game.UpdateSkip, game.Exit);
        }



        //void EngineStop () {
        //    engineWork = false;
        //
        //    Thread.Sleep(0);
        //    Console.Clear();
        //    Console.ForegroundColor = DefaultValues.text;
        //    Console.BackgroundColor = DefaultValues.cell;
        //    Console.SetCursorPosition(0, 0);
        //
        //}


        void ExitThreadEngine () {
            engineWork = false;

            //Thread.Sleep(100);
            switch (state) {
                case States.menu:
                    menu = null;

                    break;
                case States.video:
                    video.Exit();
                    video = null;

                    break;
                case States.videoFromText:
                    videoFromText = null;

                    break;
                case States.game:
                    game = null;

                    break;
            }
            state = States.none;
            //Thread.Sleep(100);

            // if (isSelfEnd) {
            //     isSelfEnd = false;
            //     Menu();
            // }

            //while (isPassing) { }
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Title = (Process.GetCurrentProcess().Threads.Count - threadsStartCount).ToString();

            isEngineWorking = false;
            //Thread.Sleep(1000);
        }
        public void ExitThreadControl () {
            ExitThreadEngine();
            appWork = false;
            Console.WriteLine("Exiting...");
            Thread.Sleep(200);
        }

    }
}
