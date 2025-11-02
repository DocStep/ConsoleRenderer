using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using static ConsoleRenderer.DefaultValues;
using static ConsoleRenderer.Input;


public enum EngineStates {
    none = -1,

    Menu,
    Video,
    VideoFromText,
    Game,
}

#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {
    public class Engine {

        public static Engine instance;

        //public static Action de_Init;
        //public static Action de_Start;
        //public static Action de_Update;
        //public static Action de_UpdateSkip;
        //public static Action de_Exit;

        public static ConsoleGame ConsoleGame;
        public static Renderer Renderer;

        public Thread th_Control;
        public Thread th_Engine;
        public Input Input;


        public Engine () {
            Init_internal();
        }
        public Engine (ConsoleGame game) {
            ConsoleGame = game;
            Engine.Renderer.Init(height, width);
        }
        void Init_internal () {
            instance = this;
            Renderer = new Renderer();
            Input = new Input();
            threadsStartCount = Process.GetCurrentProcess().Threads.Count;
            nextIterTime = DateTime.Now.Ticks + (long)(1f/fpsMax*TimeSpan.TicksPerSecond);

            th_Control = new Thread(ThreadControl);
            th_Engine = new Thread(ThreadEngine);

            Renderer.Debugger.threadsCount = Process.GetCurrentProcess().Threads.Count - threadsStartCount;
        }
        public void Start () {
            if (ConsoleGame == null) return;
            
            th_Control.Start();
            th_Engine.Start();
        }

        public EngineStates state;
        public bool engineWork;
        public bool appWork;
        public int threadsStartCount;
        public long nextIterTime;
        public int framesQueue = 0;


        public bool isEngineWorking;
        public bool isPassing;
        public bool isSelfEnd;

        public static double fpsMax = 60;



        void ThreadEngine () {
            isEngineWorking = true;
            engineWork = true;

            ConsoleGame?.Start();

            while (engineWork) {
                if (0 < framesQueue) {
                    isPassing = true;

                    // Frame skip
                    int skipped = 0;
                    while (1 < framesQueue  && skipped < 10) {
                        Renderer.Debugger.framesSkipped++;
                        //skipped++;
                        framesQueue--;

                        ConsoleGame?.Start();
                        //de_UpdateSkip?.Invoke();
                    }

                    // Frame
                    framesQueue--;

                    ConsoleGame?.Update();
                    //de_Update?.Invoke();

                    Renderer.Debugger.framesQueue = framesQueue;
                    Renderer.Pass();
                    isPassing = false;
                }
            }

            ExitThreadEngine();
        }
        void ThreadControl () {
            appWork = true;
            nextIterTime = DateTime.Now.Ticks;
            while (appWork) {
                ConsoleGame?.Keys();

                if (DateTime.Now.Ticks >= nextIterTime + (long)(1f/fpsMax*TimeSpan.TicksPerSecond)) {
                    nextIterTime += (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
                    framesQueue++;
                }
            }

            ExitThreadControl();
        }




        void ExitThreadEngine () {
            engineWork = false;
            state = EngineStates.none;

            Console.ForegroundColor = DefaultValues.c_Text;
            Console.BackgroundColor = DefaultValues.c_Cell;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            //Console.Title = (Process.GetCurrentProcess().Threads.Count - threadsStartCount).ToString();
            Console.Title = "ExitThread";

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
