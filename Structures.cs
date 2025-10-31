using System.Collections.Generic;


namespace ConsoleRenderer {
    public struct Pixel {
        public Pixel (byte grey) {
            R = grey;
            G = grey;
            B = grey;
        }
        public Pixel (byte r, byte g, byte b) {
            R = r;
            G = g;
            B = b;
        }

        byte R;
        byte G;
        byte B;
        
        public static Pixel black = new Pixel(0, 0, 0);
        public static Pixel white = new Pixel(255, 255, 255);

        public byte Gray () => (byte)((R + G + B) / 3); // simple greyscale
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
