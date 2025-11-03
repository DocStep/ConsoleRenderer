using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;


public enum EngineStates {
    none = -1,

    Menu,
    Video,
    VideoFromText,
    Game,
}


//#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {
    public static class Engine {
        public static void Init () {
            //instance = this;
            //Renderer = new Renderer();
            Input = new Input();
            threadsStartCount = Process.GetCurrentProcess().Threads.Count;
            nextIterTime = DateTime.Now.Ticks + (long)(1f/fpsMax*TimeSpan.TicksPerSecond);

            th_Control = new Thread(ThreadControl);
            //th_Engine = new Thread(ThreadEngine);

            RendererDebugger.threadsCount = Process.GetCurrentProcess().Threads.Count - threadsStartCount;
        }
        public static void Init (Scene scene) {
            Init();
            SceneManager.Current = scene;
            //Renderer.Init(height, width);
        }

        //public static Renderer renderer;

        public static Thread th_Control;
        public static Thread th_Engine;
        public static Input Input;

        public static EngineStates state;
        public static bool engineWork;
        public static bool appWork;
        public static int threadsStartCount;
        public static long nextIterTime;
        public static int framesQueue = 0;


        public static bool isEngineWorking;
        public static bool isPassing;
        public static bool isSelfEnd;

        public static double fpsMax = 60;


        public static void Start () {
            if (SceneManager.Current == null) return;
            
            th_Control.Start();
            //th_Engine.Start();
            ThreadEngine();
        }
        static void ThreadEngine () {
            isEngineWorking = true;
            engineWork = true;

            while (engineWork) {
                if (0 < framesQueue) {
                    isPassing = true;

                    // Frame skip
                    int skipped = 0;
                    while (1 < framesQueue  && skipped < 10) {
                        RendererDebugger.framesSkipped++;
                        //skipped++;
                        framesQueue--;
                        SceneManager.Current?.UpdateSkip();
                    }

                    // Frame
                    framesQueue--;
                    SceneManager.Current?.Update();

                    RendererDebugger.framesQueue = framesQueue;
                    Renderer.Pass();
                    isPassing = false;
                }
            }

            ExitThreadEngine();
        }
        static void ThreadControl () {
            appWork = true;
            nextIterTime = DateTime.Now.Ticks;
            while (appWork) {
                SceneManager.Current?.Keys();

                if (DateTime.Now.Ticks >= nextIterTime + (long)(1f/fpsMax*TimeSpan.TicksPerSecond)) {
                    nextIterTime += (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
                    framesQueue++;
                }
            }

            ExitThreadControl();
        }




        static void ExitThreadEngine () {
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
        static void ExitThreadControl () {
            ExitThreadEngine();
            appWork = false;
            Console.WriteLine("Exiting...");
            Thread.Sleep(200);
        }

    }
}
