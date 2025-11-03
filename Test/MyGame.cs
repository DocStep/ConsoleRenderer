using System;
using System.Diagnostics;
using System.Drawing;
using ConsoleRenderer;


namespace Test;

class MyGame {
    static void Main (string[] args) {
        Engine.Init();
        SceneManager.Current = new Video(@"G:/temp/apple.mp4", pixelsPerCell: 12, colors: DefaultValues.Colors4, useAscii: false);
        //SceneManager.Current = new VideoFromText(@"G:/temp/DG2HeroAnimation.txt", height: 24, width: 60, fps: 24);

        //SceneManager.Current = new GameTest_AlgBML(height: 30, width: 30, DefaultValues.Colors4);
        //Renderer.Passer = new PasserFull();
        //Engine.fpsMax = 20;

        //SceneManager.Current = new GameTest_AlgCircle(height: 30, width: 30, DefaultValues.Colors2);
        //SceneManager.Current = new GameTest_AlgRectangle(height: 30, width: 30, DefaultValues.Colors2);
        //SceneManager.Current = new GameTest_Glitch(height: 30, width: 30, DefaultValues.Colors4);

        //Renderer.Passer = new PasserColorChangeSets();
        //Renderer.Passer = new PasserFull();
        Engine.Start();
    }
}
