namespace ConsoleRenderer;

public static class RendererDebugger {

    public static bool debug = true;
    public static int delay = 0;
    public static int fps = 0;

    public static string state = "";
    public static int threadsCount = 0;
    static int writes, recolorsCell, recolorsText, cursorSets;
    static long renderStart, renderEnd, videoLatency;
    static int renderLatecy;
    public static string text = "";

    public static int fixedFramesTotal = 0;
    public static int framesTotal = 0;
    public static int framesQueue = 0;
    public static int fixedFramesSkipped = 0;
    public static int framesSkipped = 0;
    public static void SetCursor () { cursorSets++; }
    public static void SetColorCell () { recolorsCell++; }
    public static void SetColorText () { recolorsText++; }
    public static void Write () { writes++; Thread.Sleep(delay); }


    public static void DebugReset () {
        writes = 0; recolorsCell = 0; recolorsText = 0; cursorSets = 0;
        renderStart = DateTime.Now.Ticks;
    }

    public static void DebugOut () {
        //maxTop = Math.Max(maxTop, Console.CursorTop);
        //maxLeft = Math.Max(maxLeft, Console.CursorLeft);
        //Console.Title = $"{maxTop} {maxLeft}";
        //Console.Title = $"{Console.CursorTop} {Console.CursorLeft}";
        //Console.Title = $"Output redirected: {Console.IsOutputRedirected}";
        //return;

        if (debug) {
            GetFPS();
            renderEnd = DateTime.Now.Ticks;
            renderLatecy = (int)(renderEnd-renderStart)/1000;
            videoLatency += renderLatecy;
            //Console.Title = (renderer.title.Length > 0 ? $"{renderer.title} " : "") +
            Console.Title = $"{state} ({threadsCount}) {Renderer.Passer.name}" +
                $"[ fps:{fps,5} / (fF/f):({fixedFramesTotal,5}/{framesTotal,5}) / fQ:{framesQueue,4} " +
                $"/ fFS:{fixedFramesSkipped,4} / fS:{framesSkipped,4} |" +
                $"rL/vL:{renderLatecy,4}/{videoLatency/1000,4} | w:{writes,5}; cC:{recolorsCell,5}; cT:{recolorsText,5}; c:{cursorSets,5} ] {text}";
        } else Console.Title = $"{Renderer.title}";
    }


    static List<long> framesTime = new List<long>();
    static long startS;
    static void GetFPS () {
        startS = DateTime.Now.Ticks;
        framesTime.Add(DateTime.Now.Ticks);

        if (DateTime.Now.Ticks > startS + TimeSpan.TicksPerSecond) startS += TimeSpan.TicksPerSecond;

        framesTime = framesTime.Where(x => x > DateTime.Now.Ticks - TimeSpan.TicksPerSecond).ToList();
        fps = framesTime.Count-1;
    }

}
