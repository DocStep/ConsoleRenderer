using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace ConsoleRenderer {
    public class RendererDebugger {

        public Renderer renderer;

        public RendererDebugger (Renderer renderer) {
            this.renderer  = renderer;
        }

        public bool debug = true;
        public int delay = 0;
        public int fps = 0;

        public string state = "";
        public int threadsCount = 0;
        int writes, recolorsBG, recolorsText, sets;
        long renderStart, renderEnd, videoLatency;
        int renderLatecy;
        public string text = "";

        public int framesQueue = 0;
        public int framesSkipped = 0;
        public void SetCursor () { sets++; }
        public void SetColorCell () { recolorsBG++; }
        public void SetColorText () { recolorsText++; }
        public void Write () { writes++; Thread.Sleep(delay); }


        public void DebugReset () {
            writes = 0; recolorsBG = 0; recolorsText = 0; sets = 0;
            renderStart = DateTime.Now.Ticks;
        }

        public void DebugOut () {
            if (debug) {
                GetFPS();
                renderEnd = DateTime.Now.Ticks;
                renderLatecy = (int)(renderEnd-renderStart)/1000;
                videoLatency += renderLatecy;
                //Console.Title = (renderer.title.Length > 0 ? $"{renderer.title} " : "") +
                Console.Title = $"{state} ({threadsCount}) " +
                    $"[ f:{fps,5} / fT:{renderer.iter,5} / fQ:{framesQueue,4} / fS:{framesSkipped,4} | " +
                    $"rL:{renderLatecy,4} / {videoLatency/1000, 4} | w:{writes,5}; cC:{recolorsBG,5}; cT:{recolorsText,5}; c:{sets,5} ] {text}";
            //} else Console.Title = $"{renderer.title}";
            } else Console.Title = $"non";
        }


        List<long> framesTime = new List<long>();
        long startS;
        void GetFPS () {
            startS = DateTime.Now.Ticks;
            framesTime.Add(DateTime.Now.Ticks);

            if (DateTime.Now.Ticks > startS + TimeSpan.TicksPerSecond) startS += TimeSpan.TicksPerSecond;

            framesTime = framesTime.Where(x => x > DateTime.Now.Ticks - TimeSpan.TicksPerSecond).ToList();
            fps = framesTime.Count-1;
        }

    }
}
