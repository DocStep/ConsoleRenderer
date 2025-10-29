using System;
using System.Collections.Generic;
using static ConsoleRenderer.DefaultValues;


namespace ConsoleRenderer {
    public class MenuCanvas {

        public Engine engine;
        public Input input;
        //public Renderer renderer;

        public Menu menuActive, menuMain, menuVideo, menuVideoFromText, menuGame;
        public List<Menu> list = new List<Menu>();
        public string path = "";
        public int iter = 0;

        public MenuCanvas (Engine engine) {
            engine.state = States.menu;
            engine.renderer = new Renderer(40, 10, DefaultValues.colorsBW);
            engine.renderer.debugger.state = engine.state.ToString();
            engine.nextIterTime = DateTime.Now.Ticks + (long)(1f/engine.fpsMax*TimeSpan.TicksPerSecond);

            this.engine = engine;
            input = this.engine.input;
            //renderer = engine.renderer;

            Init();
        }
        void Init () {
            menuMain = new Menu("MENU", true, engine.renderer);
            menuMain.Add("Video", menuVideo, () => {
                //menuActive = menuVideo;
                //engine.Video(@"D:/temp/apple.mp4", 15, DefaultValues.colorsGs, false);
                engine.Video(@"D:/temp/apple.mp4", 20, DefaultValues.colorsGreys4, false);
            });
            menuMain.Add("Video From Text", menuVideo, () => {
                engine.VideoFromText(@"D:/DG2HeroAnimation.txt", 60, 24, 24);

                //menuActive = menuVideo;
            });
            menuMain.Add("Game", menuVideo, () => {
                //menuActive = menuVideo;

                engine.Game(60, 30, new Dictionary<int, ConsoleColor>() {
                        { -1, ConsoleColor.DarkGray },
                        { 0, ConsoleColor.Black },
                        { 1, ConsoleColor.Cyan },
                        { 2, ConsoleColor.Magenta },
                        { 3, ConsoleColor.Green },
                        { 4, ConsoleColor.DarkYellow },
                    });
            });
            menuMain.AddBack("Quit", null, () => {
                //engine.engineWork = false;
                engine.appWork = false;
                //engine.ExitThreadControl();
            });
            //menuMain.Add("Animation from text", null, () => {
            //    new VideoFromText(engine, @"D:/DG2HeroAnimation.txt", 60, 24, 24);
            //});
            //menuMain.Add("Game", null, () => {
            //    new Game(engine, 60, 30, new Dictionary<int, ConsoleColor>() {
            //        { -1, ConsoleColor.DarkGray },
            //        { 0, ConsoleColor.Black },
            //        { 1, ConsoleColor.Cyan },
            //        { 2, ConsoleColor.Magenta },
            //        { 3, ConsoleColor.Green },
            //        { 4, ConsoleColor.DarkYellow },
            //    });
            //});
            //menuMain.AddBack("Back", null, () => {
            //    engine.engineWork = false;
            //});
            //
            //
            //menuVideo = new Menu("Video Menu", true, engine.renderer);
            //menuVideo.Add("apple", null, () => {
            //    new Video(engine, @"D:/temp/apple.mp4", 20, DefaultValues.colorsGs, false);
            //});

            menuActive = menuMain;
        }


        public void Keys () {
            if (Input.GetKeyDown('W')) {
                menuMain.Select(menuMain.selected - 1);
            }
            if (Input.GetKeyDown('S')) {
                menuMain.Select(menuMain.selected + 1);
            }
            if (Input.GetKeyDown('X')) {
                engine.renderer.debugger.debug = !engine.renderer.debugger.debug;
            }

            if (Input.GetKeyDown('Q')) {
                BackAction();
            }
            if (Input.GetKeyDown(' ')) {
                SelectedAction();
            }
        }
        public void Start () {

        }
        public void Update () {
            iter++;
            menuActive.Write();


            if (engine.state == States.menu) ProcessLoadingSymbol();
        }
        public void UpdateSkip () {
            iter++;
        }
        public void Exit () {

        }




        public void SelectedAction () {
            menuActive.lines[menuActive.selected].action();
        }
        public void BackAction () {
            menuActive.back.action();
        }

        int currloadingChar = 0;
        void ProcessLoadingSymbol () {
            int frameDelta = engine.fpsMax/loading.Length/2;
            if (iter % frameDelta == 0) currloadingChar++;
            engine.renderer.Write(loading[currloadingChar % loading.Length].ToString(), menuActive.selected+2, 2, 0, 1, false);
        }

    }
}
