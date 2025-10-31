using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleRenderer {
    public class Game {

        public Engine engine;
        public Renderer renderer;

        public Game (Engine engine, int height, int width, Dictionary<int, ConsoleColor> colors) {
            engine.state = States.game;
            engine.renderer = new Renderer(height, 2*width, colors);
            
            this.engine = engine;
            //renderer = engine.renderer;
        }



        public void Keys () {
            if (Input.GetKeyDown('Q')) {
                engine.Menu();
            }

        }
        public void Start () {

        }
        public void Update () {

        }
        public void UpdateSkip () {

        }
        public void Exit () {

        }


    }
}
