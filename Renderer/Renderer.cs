using SixLabors.ImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;


#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {
    public static class Renderer { /// <> Uses grey-scale yet

        public static Passer Passer;

        public static Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>() { [0] = ConsoleColor.Black };
        public static List<int> i_colorsGs = new List<int>();
        public static string title = "Renderer Engine";

        public static bool isPassing;
        public static bool needPass = false;
        public static bool shouldInterrupt = false;
        public static int height;
        public static int width;
        public static int widthSquares;
        //public static int framesTotal = 0;
        public static bool forceAscii = false;
        public static string textAscii = "";

        public static char[,] arrText;
        public static int[,] arrTextColor;
        public static int[,] arrCellColor;

        public static char[,] bufferText;
        public static int[,] bufferTextColor;
        public static int[,] bufferCellColor;

        public static Pixel[,] cellFrameBuffer;
        public static Pixel[,] cellFrameBufferGs;


        public static void Init (int height, int width, Dictionary<int, ConsoleColor> colors = null) {
            Renderer.colors = colors != null ? colors : DefaultValues.Colors2;

            Renderer.height = height;
            Renderer.width = width;
            widthSquares = width/2;

            arrText = new char[height, width];
            arrTextColor = new int[height, width];
            arrCellColor = new int[height, width];
            pixels = new Pixel[height, width];
            pixelsGs = new float[height, width/2];

            int i_colorDef = colors.ElementAt(0).Key;
            arrText = Lib.ArrayFill(arrText, ' ');
            arrTextColor = Lib.ArrayFill(arrTextColor, i_colorDef);
            arrCellColor = Lib.ArrayFill(arrCellColor, i_colorDef);
            //videoCellsGs = Simple.ArrayFill(videoCellsGs, 0);

            bufferText = new char[height, width];
            bufferTextColor = new int[height, width];
            bufferCellColor = new int[height, width];

            i_colorsGs = colors.Keys.ToList();

            InitCanvas(height, width);
            EmptyCanvas(height, width);

            RendererDebugger.state = Engine.state.ToString();
            RendererDebugger.framesQueue = 0;
            //Renderer.RendererDebugger.threadsCount = Process.GetCurrentProcess().Threads.Count - threadsStartCount;
        }

        static void InitCanvas (int height, int width) {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
        }
        static void EmptyCanvas (int height, int width) {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.c_Text;
            Console.BackgroundColor = DefaultValues.c_Cell;
            string text = new string(' ', height*width);
            Console.Write(text);
            //Simple.WriteArray(arrText);
            
            //CellsFull();
            //CellsOverwrite();
        }


        public static void Pass () {
            isPassing = true;
            RendererDebugger.DebugReset();

            if (forceAscii) {
                SetCursor(0, 0);
                Write(textAscii);
            } else {
                //Simple.WriteArray(arrText);
                //CellsFull();
                //CellsOverwrite();
                Passer.Pass();
            }

            RendererDebugger.framesTotal++;
            RendererDebugger.DebugOut();

            ResetLayers();
            isPassing = false;
            needPass = false;
            shouldInterrupt = false;
        }
        public static int matrixSize;
        static void ResetLayers () {
            Array.Copy(arrText, bufferText, matrixSize);
            Array.Copy(arrTextColor, bufferTextColor, matrixSize);
            Array.Copy(arrCellColor, bufferCellColor, matrixSize);

            arrText = Lib.ArrayFill(arrText, ' ');
            int colorDef = i_getColorDef();
            arrTextColor = Lib.ArrayFill(arrTextColor, colorDef);
            arrCellColor = Lib.ArrayFill(arrCellColor, colorDef);
        }
        public static int i_getColorDef () {
            if (colors.Count == 0) return -1;
            return colors.ElementAt(0).Key;
        }


        public static Pixel[,] pixels;
        public static float[,] pixelsGs;
        public static float blurPower = 0;

        static void VideoFramePixels (Pixel[,] frame) {
            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthSquares; left++) {
                    Pixel pixel = frame[top, left];
                    pixels[top, left] = pixel;
                    pixelsGs[top, left] = ((pixel.R*0.3f) + (pixel.G*0.59f) + (pixel.B*0.11f))/256f;
                }
            }
        }

        public static void VideoToArrays (Pixel[,] frame) {
            if (0 < blurPower) frame = VideoFrameBlur(frame, blurPower);

            VideoFramePixels(frame);

            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthSquares; left++) {
                    int idx = (int)(pixelsGs[top, left]*colors.Count);
                    int i_color = i_colorsGs[idx];
                    arrCellColor[top, 2*left] = i_color;
                    arrCellColor[top, 2*left+1] = i_color;

                    //arrText[top, 2*left] = asciiChars[(int)((1-grayScale)*(asciiChars.Length-1))];
                    //arrText[top, 2*left+1] = asciiChars[(int)((1-grayScale)*(asciiChars.Length-1))];

                    //AsciiFromString(top, left, grayScale, 1f/colors.Count);
                    //AsciiFromDictionary(top, left, grayScale, 1f/colors.Count);
                }
            }
        }
        public static void VideoToAscii (Pixel[,] frame) {
            VideoFramePixels(frame);

            if (0 < blurPower)  VideoFrameBlur(frame, blurPower);

            textAscii = "";
            int width = (int)(0.5*Renderer.width);
            for (int top = 0; top < height; top++) {
                for (int left = 0; left < width; left++) {
                    float greyScale = pixelsGs[top, left];
                    int idx = (int)(greyScale*colors.Count);
                    /// <>
                    string cellText = asciiChars[asciiChars.Length-1-(int)(greyScale*asciiChars.Length)].ToString();
                    //string cellText = asciiChars[(int)((1-grey)*asciiChars.Length)-1].ToString();
                    textAscii += cellText + cellText;
                }
            }
        }

        static Pixel[,] VideoFrameBlur (Pixel[,] frame, float power) {
            int radius = Math.Max(1, (int)power);
            float strength = 1f/((2*radius + 1)*(2*radius + 1));

            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthSquares; left++) {
                    float r = 0;
                    float g = 0;
                    float b = 0;
                    for (int topK = -radius; topK <= radius; topK++) {
                        int topS = Math.Clamp(top + topK, 0, height - 1);
                        for (int leftK = -radius; leftK <= radius; leftK++) {
                            int leftS = Math.Clamp(left + leftK, 0, widthSquares - 1);
                            Pixel p = frame[topS, leftS];
                            r += p.R;
                            g += p.G;
                            b += p.B;
                        }
                    }

                    cellFrameBufferGs[top, left].R = (byte)(strength*r);
                    cellFrameBufferGs[top, left].G = (byte)(strength*g);
                    cellFrameBufferGs[top, left].B = (byte)(strength*b);
                }
            }

            //frame = videoFrameBufferGs;
            return frame = cellFrameBufferGs;
        }


        static void AsciiFromString (int top, int left, float grayScale, float treshold) {
            int idx = (int)(grayScale*colors.Count);
            int i_color = colors.ElementAt(idx).Key;
            arrCellColor[top, 2*left] = i_color;
            arrCellColor[top, 2*left+1] = i_color;

            bool asciiUse = true;
            if (asciiUse) {
                if (0.5f-treshold < grayScale && grayScale < 0.5f+treshold) {
                    arrText[top, 2*left] = asciiCharsShort[(int)(grayScale*asciiCharsShort.Length)];
                    arrText[top, 2*left+1] = asciiCharsShort[(int)(grayScale*asciiCharsShort.Length)];
                    if (grayScale > 0) {
                        arrTextColor[top, 2*left] = colors.ElementAt(colors.Count-1-idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(colors.Count-1-idx).Key;
                    } else {
                        arrTextColor[top, 2*left] = i_color;
                        arrTextColor[top, 2*left+1] = i_color;
                    }
                } else {
                    arrText[top, 2*left] = ' ';
                    arrText[top, 2*left+1] = ' ';
                }
            }
        }
        static void AsciiFromDictionary (int top, int left, float grayScale, float treshold) {
            int idx = (int)(grayScale*colors.Count());
            int i_color = colors.ElementAt(idx).Key;
            arrCellColor[top, 2*left] = i_color;
            arrCellColor[top, 2*left+1] = i_color;

            //float treshold = 1f/colors.Count;
            bool asciiUse = true;
            if (asciiUse) {
                if (0.5f-treshold < grayScale && grayScale < 0.5f+treshold) {
                    float inTresholdScale = (treshold - 0.5f + grayScale)/(2*treshold);
                    int idx_ascii = (int)(inTresholdScale*DefaultValues.videoCellPairChars.Count());

                    arrText[top, 2*left] = DefaultValues.videoCellPairChars.ElementAt(idx_ascii).Key;
                    arrText[top, 2*left+1] = DefaultValues.videoCellPairChars.ElementAt(idx_ascii).Value;

                    if (0.5f < grayScale) {
                        arrTextColor[top, 2*left] = colors.ElementAt(colors.Count-1-idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(colors.Count-1-idx).Key;
                    } else {
                        arrTextColor[top, 2*left] = i_color;
                        arrTextColor[top, 2*left+1] = i_color;
                    }
                } else {
                    arrText[top, 2*left] = ' ';
                    arrText[top, 2*left+1] = ' ';
                    arrTextColor[top, 2*left] = i_color;
                    arrTextColor[top, 2*left+1] = i_color;
                }
            }
        }


        public static string asciiChars = "█$@B%8WM#Z0QOCYhkbdpqwmzaocvunxrjft*/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
        public static string asciiCharsShort = "█$@B%8WM#Z0QOCJYXhkbpmwzcvunxrj*+~-'. ";
        public static char GetAsciiCharForValue (int value) {
            return asciiChars[value*asciiChars.Length];
        }




        /* Text To Buffer */
        public static void Write (string text, int top, int left, int colorText, int colorCell, bool trimEndLine) {
            if (trimEndLine) text = text.Substring(0, arrCellColor.GetLength(1)-left);

            while (0 < text.Length) {
                if (width-1 < left) {
                    if (trimEndLine || height <= top) break;
                    else {
                        left = 0;
                        top++;
                    }
                }
                if (height <= top || width <= left) break;

                string line;
                int l = left % 2;
                if (l == 0) {
                    int tl = Math.Clamp(2, 0, text.Length);
                    line = text.Substring(0, tl);
                    text = text.Substring(tl, Math.Clamp(text.Length-2, 0, text.Length));
                } else {
                    line = text.Substring(0, 1);
                    text = text.Substring(1, text.Length-1);
                }

                if (1 <= line.Length) {
                    int margin = 0;
                    arrText[top, left+margin] = line[margin];
                    arrTextColor[top, left+margin] = colorText;
                    arrCellColor[top, left+margin] = colorCell;
                }
                if (line.Length == 2) {
                    int margin = 1;
                    arrText[top, left+margin] = line[margin];
                    arrTextColor[top, left+margin] = colorText;
                    arrCellColor[top, left+margin] = colorCell;
                }

                left += line.Length;
            }
        }





        public static void WriteChangeSet (List<CellDiffPos> groups) {
            int groupsCount = groups.Count;
            for (int iColor = 0; iColor < groupsCount; iColor++) {
                CellDiffPos group = groups[iColor];
                SetColorText(group.colorText);
                SetColorCell(group.colorCell);
                int posCount = group.pos.Count;
                for (int iText = 0; iText < posCount; iText++)
                    DrawCellsChange(group.pos[iText]);
            }
        }
        static void WriteChangeSet_Interruption (List<CellDiffPos> groups) {
            int groupsCount = groups.Count;
            for (int iColor = 0; iColor < groupsCount; iColor++) {
                /// Interrupt Check
                /// <> <mb> Interruption for ChangeSets has problem of impossibility to track actual changes
                /// need complex tracking algorithm
                if (Renderer.shouldInterrupt) {
                    Renderer.shouldInterrupt = false;
                    RendererDebugger.framesSkipped++;
                    return;
                }

                CellDiffPos group = groups[iColor];
                SetColorText(group.colorText);
                SetColorCell(group.colorCell);
                int posCount = group.pos.Count;
                for (int iText = 0; iText < posCount; iText++)
                    DrawCellsChange(group.pos[iText]);
            }
        }
        public static void DrawCellsChange (Pos pos) {
            SetCursor(pos.top, pos.left);
            Write(pos.text);
        }
        public static string cellText (int top, int left) {
            return string.Empty + arrText[top, left];
        }



        /* Simple */
        static int colorCell = 0;
        static int colorText = 0;
        static int cursorTop = 0;
        static int cursorLeft = 0;
        public static void Write (string s) {
            Console.Write(s);
            RendererDebugger.Write();
        }
        public static void WriteLine (string s) {
            Console.WriteLine(s);
            RendererDebugger.Write();
        }
        public static void SetColorCell (ConsoleColor colorCell) {
            int colorKey = colors.First(x => x.Value == colorCell).Key;
            if (Renderer.colorCell != colorKey) {
                Renderer.colorCell = colorKey;
                Console.BackgroundColor = colorCell;
                RendererDebugger.SetColorCell();
            }
        }
        public static void SetColorCell (int colorCell) {
            if (Renderer.colorCell != colorCell) {
                Renderer.colorCell = colorCell;
                Console.BackgroundColor = colors.First(x => x.Key == colorCell).Value;
                //Console.BackgroundColor = colors[colorCell];
                RendererDebugger.SetColorCell();
            }
        }
        // add "space" string check
        public static void SetColorText (ConsoleColor colorText) {
            int colorKey = colors.First(x => x.Value == colorText).Key;
            if (Renderer.colorText != colorKey) {
                Renderer.colorText = colorKey;
                Console.ForegroundColor = colorText;
                RendererDebugger.SetColorText();
            }
        }
        public static void SetColorText (int colorText) {
            if (Renderer.colorText != colorText) {
                Renderer.colorText = colorText;
                Console.ForegroundColor = colors.First(x => x.Key == colorText).Value;
                RendererDebugger.SetColorText();
            }
        }
        public static void SetCursor (int top, int left) {
            if (top != Console.GetCursorPosition().Top || left != Console.GetCursorPosition().Left) {
                cursorTop = Console.GetCursorPosition().Top;
                cursorLeft = Console.GetCursorPosition().Left;
                //Console.SetWindowSize(width, height);
                //if (left < Console.WindowWidth && left < Console.WindowHeight) 
                Console.SetCursorPosition(left, top);
                RendererDebugger.SetCursor();
            }
        }

    }
}
