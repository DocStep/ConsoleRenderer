using System;
using System.Collections.Generic;


namespace ConsoleRenderer {

    public class Menu {

        public class Line {
            public Line (string name, Menu menu, Action action) {
                this.name = name;
                this.menu = menu;
                this.action = action;
            }

            public string name;
            public Menu menu;
            public Action action;
        }

        Renderer renderer;

        public Menu (string name, bool main, Renderer renderer) {
            this.name = name;
            this.main = main;
            this.renderer = renderer;

            renderer.Write(name, 0, 2, 1, 0, false);
        }


        public bool main;
        public string name = "";
        public List<Line> lines = new List<Line>();
        public int selected = 0;
        public Line back = null;


        public void Add (string text, Menu menu, Action action) {
            lines.Add(new Line($"{lines.Count+1}. {text}", menu, action));
            //renderer.Write(lines[lines.Count-1].Key, lines.Count+1, 0, 1, 0, false);
        }

        public void AddBack (string text, Menu menu, Action action) {
            back = new Line("[Q] " + text, menu, action);
            //renderer.Write(lines[lines.Count-1].Key, lines.Count+1, 0, 1, 0, false);
        }

        public void Select (int index) {
            index = Math.Clamp(index, 0, lines.Count-1);
            if (selected >= 0) renderer.Write(lines[selected].name, selected+2, 0, 1, 0, false);
            renderer.Write(lines[index].name, index+2, 0, 0, 1, false);
            selected = index;
        }

        public void Write () {
            renderer.Write(name, 0, 2, 1, 0, false);
            for (int i = 0; i < lines.Count; i++) {
                if (i == selected) {
                    renderer.Write(lines[i].name, i+2, 0, 0, 1, false);
                } else {
                    renderer.Write(lines[i].name, i+2, 0, 1, 0, false);
                }
            }
            renderer.Write(back.name, lines.Count+3, 0, 1, 0, false);
        }

    }



    /* Structures */

    struct CellDiffPos {
        public CellDiffPos (int colorText, int colorCell, Pos cellPos) {
            this.colorText = colorText;
            this.colorCell = colorCell;
            pos = new() { cellPos };
        }

        public int colorText = 0;
        public int colorCell = 0;

        public List<Pos> pos = new();
    }

    public class Pos {
        public Pos (string text, int top, int left) {
            this.text = text;
            this.top = top;
            this.left = left;
        }

        public string text = "";
        public int top = 0;
        public int left = 0;
    }


    
    struct TextDiffPos {
        public TextDiffPos (int colorText, int colorCell, Pos textPos) {
            this.colorText = colorText;
            this.colorCell = colorCell;
            pos = new() { textPos };
        }

        public int colorText = 0;
        public int colorCell = 0;

        public List<Pos> pos = new();
    }

}
