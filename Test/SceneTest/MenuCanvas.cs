using ConsoleRenderer;
using System;
using System.Collections.Generic;


namespace Test {
    public class MenuCanvas : Scene {
        public MenuCanvas () {
            Engine.state = EngineStates.Menu;

            Renderer.Init(10, 40, DefaultValues.Colors2);
            Engine.nextIterTime = DateTime.Now.Ticks + (long)(1f/Engine.fpsMax*TimeSpan.TicksPerSecond);

            Init();
        }

        public Menu menuActive, menuMain, menuVideo, menuVideoFromText, menuGame;
        public List<Menu> list = new List<Menu>();
        public string path = "";
        public int iter = 0;


        public void Init () {
            menuMain = new Menu("MENU", true);
            menuMain.Add("Video", menuVideo, (Action)(() => {
                SceneManager.Current = new Video(@"G:/temp/apple.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);
            }));
            menuMain.Add("Video From Text", menuVideo, (Action)(() => {
                SceneManager.Current = new VideoFromText(@"G:/temp/DG2HeroAnimation.txt", 24, 60, 24);
                //menuActive = menuVideo;
            }));
            menuMain.Add("Game", menuVideo, (Action)(() => {
                //menuActive = menuVideo;

                /*engine.Game(60, 30, new Dictionary<int, ConsoleColor>() {
                        { -1, ConsoleColor.DarkGray },
                        { 0, ConsoleColor.Black },
                        { 1, ConsoleColor.Cyan },
                        { 2, ConsoleColor.Magenta },
                        { 3, ConsoleColor.Green },
                        { 4, ConsoleColor.DarkYellow },
                    });*/

                Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>() {
                    { -1, ConsoleColor.DarkGray },
                    { 0, ConsoleColor.Black },
                    { 1, ConsoleColor.Cyan },
                    { 2, ConsoleColor.Magenta },
                    { 3, ConsoleColor.Green },
                    { 4, ConsoleColor.DarkYellow },
                };
                SceneManager.Current = new GameTest_AlgBML(30, 30, colors: colors);
            }));
            menuMain.AddBack("Quit", null, () => {
                //engine.engineWork = false;
                //engine.appWork = false;
                Engine.appWork = false;
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


        public override void Keys () {
            if (Input.GetKeyDown('W')) {
                menuMain.Select(menuMain.selected - 1);
            }
            if (Input.GetKeyDown('S')) {
                menuMain.Select(menuMain.selected + 1);
            }
            if (Input.GetKeyDown('X')) {
                RendererDebugger.debug = !RendererDebugger.debug;
            }

            if (Input.GetKeyDown('Q')) {
                BackAction();
            }
            if (Input.GetKeyDown(' ')) {
                SelectedAction();
            }
        }


        public override void Update () {
            iter++;
            menuActive.Write();

            //if (Engine.instance.state == EngineStates.Menu) 
            ProcessLoadingSymbol();
        }
        public override void UpdateSkip () {
            iter++;
        }


        public void SelectedAction () {
            menuActive.lines[menuActive.selected].action();
        }
        public void BackAction () {
            menuActive.back.action();
        }

        int currloadingChar = 0;
        void ProcessLoadingSymbol () {
            int frameDelta = (int)(Engine.fpsMax/DefaultValues.loading.Length/2);
            if (iter % frameDelta == 0) currloadingChar++;
            Renderer.Write(DefaultValues.loading[currloadingChar % DefaultValues.loading.Length].ToString(), menuActive.selected+2, 2, 0, 1, false);
        }

    }
}
