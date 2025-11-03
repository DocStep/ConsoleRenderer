using System.Collections.Generic;


/// Structures
namespace ConsoleRenderer;

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

    public byte R;
    public byte G;
    public byte B;
        
    public static Pixel black = new Pixel(0, 0, 0);
    public static Pixel white = new Pixel(255, 255, 255);

    public byte Gray () => (byte)((R + G + B) / 3); // simple greyscale


    public static Pixel operator + (Pixel pixel1, Pixel pixel2) {
        pixel1.R = (byte)(pixel1.R + pixel2.R);
        pixel1.G = (byte)(pixel1.G + pixel2.G);
        pixel1.B = (byte)(pixel1.B + pixel2.B);
        return pixel1;
    }
    public static Pixel operator - (Pixel pixel1, Pixel pixel2) {
        pixel1.R = (byte)(pixel1.R - pixel2.R);
        pixel1.G = (byte)(pixel1.G - pixel2.G);
        pixel1.B = (byte)(pixel1.B - pixel2.B);
        return pixel1;
    }
    public static Pixel operator * (Pixel pixel, float value) {
        pixel.R = (byte)(pixel.R*value);
        pixel.G = (byte)(pixel.G*value);
        pixel.B = (byte)(pixel.B*value);
        return pixel;
    }
    public static Pixel operator / (Pixel pixel, float value) {
        pixel.R = (byte)(pixel.R/value);
        pixel.G = (byte)(pixel.G/value);
        pixel.B = (byte)(pixel.B/value);
        return pixel;
    }

}


public struct CellDiffPos {
    public CellDiffPos (int colorText, int colorCell, TextPos cellPos) {
        this.colorText = colorText;
        this.colorCell = colorCell;
        pos = new() { cellPos };
    }

    public int colorText = 0;
    public int colorCell = 0;

    public List<TextPos> pos = new();
}

public class TextPos {
    public TextPos (string text, int top, int left) {
        this.text = text;
        this.top = top;
        this.left = left;
    }

    public string text = "";
    public int top = 0;
    public int left = 0;
}

struct TextDiffPos {
    public TextDiffPos (int colorText, int colorCell, TextPos textPos) {
        this.colorText = colorText;
        this.colorCell = colorCell;
        pos = new() { textPos };
    }

    public int colorText = 0;
    public int colorCell = 0;

    public List<TextPos> pos = new();
}
