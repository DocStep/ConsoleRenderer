using System.Diagnostics;


namespace ConsoleRenderer;

public enum EngineStates {
    none = -1,

    Menu,
    Video,
    VideoFromText,
    Game,
}


public static class Engine {
    public static void Init () {
        Input = new Input();
        threadsStartCount = Process.GetCurrentProcess().Threads.Count;
        nextIterTime = DateTime.Now.Ticks + (long)(1f/fpsMax*TimeSpan.TicksPerSecond);

        th_Control = new Thread(Thread_Control);
        th_FixedUpdate = new Thread(Thread_FixedUpdate);
        th_Update = new Thread(Thread_Update);

        RendererDebugger.threadsCount = Process.GetCurrentProcess().Threads.Count - threadsStartCount;
    }
    public static void Init (Scene scene) {
        Init();
        SceneManager.Current = scene;
    }

    public static Thread th_Control;
    public static Thread th_FixedUpdate;
    public static Thread th_Update;

    public static Input Input;

    /// States
    public static EngineStates state = EngineStates.none;
    public static bool engineWork = true;
    public static bool appWork;
    public static int threadsStartCount;
    public static long nextIterTime;
    //public static int framesQueue = 0;
    //public static bool fixedFrameEnded;


    //public static bool isEngineWorking;
    //public static bool isPassing;
    //public static bool isSelfEnd;

    public static double fpsMax = 60;


    public static void Start () {
        if (SceneManager.Current == null) return;

        Renderer.Validation();

        engineWork = true;
        th_Control.Start();
        th_FixedUpdate.Start();
        th_Update.Start();
    }
    static void Thread_FixedUpdate () {
        while (engineWork) {
            if (0 < RendererDebugger.framesQueue) {
                /// Frame skip
                int skipped = 0;
                while (1 < RendererDebugger.framesQueue && skipped < 10) {
                    RendererDebugger.framesQueue--;
                    RendererDebugger.fixedFramesSkipped++;
                    SceneManager.Current?.FixedUpdate_Skip();
                }

                /// Frame
                RendererDebugger.framesQueue--;
                SceneManager.Current?.FixedUpdate();
                RendererDebugger.fixedFramesTotal++;

                Renderer.needPass = true;
                //Renderer.shouldInterrupt = Renderer.isPassing;

                Renderer.Pass();
            }
        }
    }

    static void Thread_Update () {
        while (engineWork) {
            if (Renderer.needPass) {
                //Renderer.Pass();
            }
        }
    }

    static void Thread_Update_Interruption () {
        while (engineWork) {
            if (!Renderer.isPassing) {
                Renderer.Pass();
            }
        }
    }

    static void Thread_Control () {
        //appWork = true;
        nextIterTime = DateTime.Now.Ticks;
        while (engineWork) {
            SceneManager.Current?.Inputs();

            if (DateTime.Now.Ticks >= nextIterTime + (long)(1f/fpsMax*TimeSpan.TicksPerSecond)) {
                nextIterTime += (long)(1f/fpsMax*TimeSpan.TicksPerSecond);
                RendererDebugger.framesQueue++;
            }
        }
    }
}
