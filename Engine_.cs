using MediaToolkit;
using System;
using System.Collections.Generic;
using System.Threading;
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
    public class Engine_ {

        public ConsoleGame consoleGame;
        public Renderer renderer;
        public Thread mainThread, controlThread;
        public Input input;

        public MenuCanvas menu;
        public Video video;
        public VideoFromText videoFromText;
        public Game game;

        public delegate void Void ();
        public Void dStart;
        public Void dUpdate;
        public Void Start;
        public Void Update;
        public Void UpdateSkip;


        public Engine_ (ConsoleGame game) {
            consoleGame = game;
            dStart += game.Start;
            dUpdate += game.Update;
            input = new Input();
        }
        void EngineStart (Void start, Void update, Void updateSkip) {
            Start = start;
            Update = update;
            UpdateSkip = updateSkip;

            engineWork = true;
            nextIterTime = DateTime.Now.Ticks + (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
            controlThread = new Thread(ThreadControl);
            controlThread.Start();
            mainThread = new Thread(ThreadEngine);
            mainThread.Start();
        }


        public States state;
        public bool engineWork = true;
        public bool appWork = true;
        public long nextIterTime;
        public int framesQueue = 0;


        public bool isEngineWorking;
        public bool isPassing;
        public int fpsMax = 60;



        void ThreadEngine () {
            isEngineWorking = true;

            if (Start != null) {
                isPassing = true;
                Start();
                if (state == States.game) dStart();

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

                    if (Update != null) Update();
                    if (state == States.game) dUpdate();

                    renderer.debugger.framesQueue = framesQueue;
                    renderer.Pass();
                    isPassing = false;
                }
            }

            ExitThreadEngine();
        }
        void ThreadControl () {
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
                    if (GetKeyDown('Q')) {
                        Menu();
                    }

                    break;
                case States.videoFromText:
                    if (GetKeyDown('Q')) {
                        Menu();
                    }

                    break;
                case States.game:
                    if (GetKeyDown('Q')) {
                        Menu();
                    }

                    break;
            }

        }



        public void Menu () {
            EngineStop();

            menu = new MenuCanvas(this);
            renderer = menu.engine.renderer;

            EngineStart(null, menu.Update, menu.UpdateSkip);
        }
        
        public void Video (string path, int pixelsPerCell, Dictionary<int, ConsoleColor> colors, bool useAscii) {
            EngineStop();

            video = new Video(this, path, pixelsPerCell, DefaultValues.colorsGs, useAscii);
            renderer = video.engine.renderer;

            EngineStart(null, video.Update, video.UpdateSkip);
        }

        public void VideoFromText (string path, int width, int height, int fps) {
            EngineStop();

            videoFromText = new VideoFromText(this, path, width, height, fps);
            renderer = videoFromText.engine.renderer;

            EngineStart(null, videoFromText.Update, videoFromText.UpdateSkip);
        }
        
        public void Game (int width, int height, Dictionary<int, ConsoleColor> colors) {
            EngineStop();

            game = new Game(this, width, height, colors);
            renderer = game.engine.renderer;

            EngineStart(game.Start, game.Update, game.UpdateSkip);
        }



        void EngineStop () {
            engineWork = false;

            Thread.Sleep(0);
            Console.Clear();
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            Console.SetCursorPosition(0, 0);
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

            while (isPassing && renderer.isPassing) { }
        }


        void ExitThreadEngine () {
            engineWork = false;
            Console.Clear();
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            Console.SetCursorPosition(0, 0);

            isEngineWorking = false;
        }
        public void ExitThreadControl () {
            ExitThreadEngine();
            appWork = false;
            Console.WriteLine("Exiting...");
            Thread.Sleep(1000);
        }

    }
}
