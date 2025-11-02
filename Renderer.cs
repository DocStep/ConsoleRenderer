using SixLabors.ImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;


#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {

    /// <> Uses grey-scale yet
    public class Renderer {

        public static RendererDebugger Debugger;

        public Renderer () {
            Renderer.Debugger = new RendererDebugger(this);
        }
        public Renderer (int height, int width, Dictionary<int, ConsoleColor> colors) {
            Renderer.Debugger = new RendererDebugger(this);
            this.colors = colors;
            Init(height, width);
        }
        public Renderer (int height, int width) {
            Renderer.Debugger = new RendererDebugger(this);
            colors = DefaultValues.Colors2;
            Init(height, width);
        }

        public void Init (int height, int width) {
            this.height = height;
            this.width = width;
            widthVideo = width/2;

            arrText = new char[height, width];
            arrTextColor = new int[height, width];
            arrCellColor = new int[height, width];
            pixels = new Pixel[height, width];
            pixelsGs = new float[height, width/2];

            int i_colorDef = colors.ElementAt(0).Key;
            arrText = lib.ArrayFill(arrText, ' ');
            arrTextColor = lib.ArrayFill(arrTextColor, i_colorDef);
            arrCellColor = lib.ArrayFill(arrCellColor, i_colorDef);
            //videoCellsGs = Simple.ArrayFill(videoCellsGs, 0);

            bufferText = new char[height, width];
            bufferTextColor = new int[height, width];
            bufferCellColor = new int[height, width];

            i_colorsGs = colors.Keys.ToList();

            InitCanvas(height, width);
            EmptyCanvas(height, width);

            Renderer.Debugger.state = Engine.instance.state.ToString();
            Renderer.Debugger.framesQueue = 0;
        }

        void InitCanvas (int height, int width) {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
        }
        void EmptyCanvas (int height, int width) {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.c_Text;
            Console.BackgroundColor = DefaultValues.c_Cell;
            string text = new string(' ', height*width);
            Console.Write(text);
            //Simple.WriteArray(arrText);
            
            CellsFull();
            CellsOverwrite();
        }



        public Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>() { [0] = ConsoleColor.Black };
        public List<int> i_colorsGs = new List<int>();
        public string title = "Renderer Engine";

        public bool isPassing;
        public int height;
        public int width;
        public int widthVideo;
        public int framesTotal = 0;
        public bool forceAscii = false;
        public string textAscii = "";

        public char[,] arrText;
        public int[,] arrTextColor;
        public int[,] arrCellColor;

        public char[,] bufferText;
        public int[,] bufferTextColor;
        public int[,] bufferCellColor;

        public Pixel[,] videoFrameBuffer;
        public Pixel[,] videoFrameBufferGs;



        public void Pass () {
            isPassing = true;
            framesTotal++;
            Renderer.Debugger.DebugReset();

            if (forceAscii) {
                SetCursor(0, 0);
                Write(textAscii);
            } else {
                //Simple.WriteArray(arrText);
                //CellsFull();
                CellsOverwrite();
            }


            Renderer.Debugger.DebugOut();

            ResetLayers();
            isPassing = false;
        }

        void ResetLayers () {
            Array.Copy(arrText, bufferText, arrText.Length);
            Array.Copy(arrTextColor, bufferTextColor, arrTextColor.Length);
            Array.Copy(arrCellColor, bufferCellColor, arrCellColor.Length);

            arrText = lib.ArrayFill(arrText, ' ');
            int colorDef = i_getColorDef();
            arrTextColor = lib.ArrayFill(arrTextColor, colorDef);
            arrCellColor = lib.ArrayFill(arrCellColor, colorDef);
        }
        public int i_getColorDef () {
            if (colors.Count == 0) return -1;
            return colors.ElementAt(0).Key;
        }


        public Pixel[,] pixels;
        public float[,] pixelsGs;
        public float blurPower = 0;

        void VideoFramePixels (Pixel[,] frame) {
            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthVideo; left++) {
                    Pixel pixel = frame[top, left];
                    pixels[top, left] = pixel;
                    pixelsGs[top, left] = ((pixel.R*0.3f) + (pixel.G*0.59f) + (pixel.B*0.11f))/256f;
                }
            }
        }

        public void VideoToArrays (Pixel[,] frame) {
            if (0 < blurPower) frame = VideoFrameBlur(frame, blurPower);

            VideoFramePixels(frame);

            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthVideo; left++) {
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
        public void VideoToAscii (Pixel[,] frame) {
            VideoFramePixels(frame);

            if (0 < blurPower)  VideoFrameBlur(frame, blurPower);

            textAscii = "";
            int width = (int)(0.5*this.width);
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

        Pixel[,] VideoFrameBlur (Pixel[,] frame, float power) {
            int radius = Math.Max(1, (int)power);
            float strength = 1f/((2*radius + 1)*(2*radius + 1));

            for (int top = 0; top < height; top++) {
                for (int left = 0; left < widthVideo; left++) {
                    float r = 0;
                    float g = 0;
                    float b = 0;
                    for (int topK = -radius; topK <= radius; topK++) {
                        int topS = Math.Clamp(top + topK, 0, height - 1);
                        for (int leftK = -radius; leftK <= radius; leftK++) {
                            int leftS = Math.Clamp(left + leftK, 0, widthVideo - 1);
                            Pixel p = frame[topS, leftS];
                            r += p.R;
                            g += p.G;
                            b += p.B;
                        }
                    }

                    videoFrameBufferGs[top, left].R = (byte)(strength*r);
                    videoFrameBufferGs[top, left].G = (byte)(strength*g);
                    videoFrameBufferGs[top, left].B = (byte)(strength*b);
                }
            }

            //frame = videoFrameBufferGs;
            return frame = videoFrameBufferGs;
        }


        void AsciiFromString (int top, int left, float grayScale, float treshold) {
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
        void AsciiFromDictionary (int top, int left, float grayScale, float treshold) {
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
        public void Write (string text, int top, int left, int colorText, int colorCell, bool trimEndLine) {
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



        /// Full Cells Render
        public void CellsFull () {
            CellsFullGroups();
            WriteChangeSet(groupsToWriteFull);
        }
        List<CellDiffPos> groupsToWriteFull = new List<CellDiffPos>();
        void CellsFullGroups () {
            groupsToWriteFull.Clear();
            int prevSymbolWritenToGroupIndex = -1;
            for (int top = 0; top < height; top++)
                for (int left = 0; left < width; left++) {
                    // check group by color
                    bool groupFound = false;
                    int textColor = arrTextColor[top, left];
                    int cellColor = arrCellColor[top, left];
                    for (int iColor = 0; iColor < groupsToWriteFull.Count; iColor++) {
                        CellDiffPos group = groupsToWriteFull[iColor];
                        if (textColor == group.colorText && cellColor == group.colorCell) {
                            groupFound = true;
                            if (prevSymbolWritenToGroupIndex == iColor) {
                                groupsToWriteFull[iColor].pos[group.pos.Count-1].text += cellText(top, left);
                            } else {
                                groupsToWriteFull[iColor].pos.Add(new Pos(cellText(top, left), top, left));
                            }
                            prevSymbolWritenToGroupIndex = iColor;
                            break;
                        }
                    }

                    if (!groupFound) {
                        prevSymbolWritenToGroupIndex = groupsToWriteFull.Count;
                        groupsToWriteFull.Add(new(textColor, cellColor, new Pos(cellText(top, left), top, left)));
                    }
                }
        }


        /// OverWrite Cells Render
        public void CellsOverwrite () {
            CellsOverwriteGroups();
            WriteChangeSet(groupsToWriteOver);
        }
        List<CellDiffPos> groupsToWriteOver = new List<CellDiffPos>();
        void CellsOverwriteGroups () {
            groupsToWriteOver.Clear();
            int prevSymbolWritenToGroupIndex = -1;
            for (int top = 0; top < height; top++)
                for (int left = 0; left < width; left++) {
                    int textColor = arrTextColor[top, left];
                    int cellColor = arrCellColor[top, left];

                    if (arrText[top, left] != bufferText[top, left] ||
                        //(arrText[top, left] == bufferText[top, left] && prevSymbolWritenToGroupIndex >= 0) || 
                        textColor != bufferTextColor[top, left] ||
                        cellColor != bufferCellColor[top, left]) {
                        // check group by colors
                        bool groupFounded = false;
                        for (int iColor = 0; iColor < groupsToWriteOver.Count; iColor++) {
                            CellDiffPos group = groupsToWriteOver[iColor];
                            if (group.colorText == textColor && group.colorCell == cellColor
                                ) {
                                groupFounded = true;
                                if (prevSymbolWritenToGroupIndex == iColor) {
                                    groupsToWriteOver[iColor].pos[group.pos.Count-1].text
                                        += cellText(top, left);
                                } else {
                                    groupsToWriteOver[iColor].pos.Add(new Pos(cellText(top, left), top, left));
                                }
                                prevSymbolWritenToGroupIndex = iColor;
                                break;
                            }
                        }

                        // create group
                        if (!groupFounded) {
                            prevSymbolWritenToGroupIndex = groupsToWriteOver.Count;
                            groupsToWriteOver.Add(new(textColor, cellColor,
                                new Pos(cellText(top, left), top, left)));
                        }
                    } else {
                        prevSymbolWritenToGroupIndex = -1;
                    }
                }
        }



        void WriteChangeSet (List<CellDiffPos> groups) {
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
        void DrawCellsChange (Pos pos) {
            SetCursor(pos.top, pos.left);
            Write(pos.text);
        }
        string cellText (int top, int left) {
            return string.Empty + arrText[top, left];
        }



        /* Simple */
        int colorCell = 0;
        int colorText = 0;
        int cursorTop = 0;
        int cursorLeft = 0;
        public void Write (string s) {
            Console.Write(s);
            Debugger.Write();
        }
        public void WriteLine (string s) {
            Console.WriteLine(s);
            Debugger.Write();
        }
        public void SetColorCell (ConsoleColor colorCell) {
            int colorKey = colors.First(x => x.Value == colorCell).Key;
            if (this.colorCell != colorKey) {
                this.colorCell = colorKey;
                Console.BackgroundColor = colorCell;
                Debugger.SetColorCell();
            }
        }
        public void SetColorCell (int colorCell) {
            if (this.colorCell != colorCell) {
                this.colorCell = colorCell;
                Console.BackgroundColor = colors.First(x => x.Key == colorCell).Value;
                //Console.BackgroundColor = colors[colorCell];
                Debugger.SetColorCell();
            }
        }
        // add "space" string check
        public void SetColorText (ConsoleColor colorText) {
            int colorKey = colors.First(x => x.Value == colorText).Key;
            if (this.colorText != colorKey) {
                this.colorText = colorKey;
                Console.ForegroundColor = colorText;
                Debugger.SetColorText();
            }
        }
        public void SetColorText (int colorText) {
            if (this.colorText != colorText) {
                this.colorText = colorText;
                Console.ForegroundColor = colors.First(x => x.Key == colorText).Value;
                Debugger.SetColorText();
            }
        }
        public void SetCursor (int top, int left) {
            if (top != Console.GetCursorPosition().Top || left != Console.GetCursorPosition().Left) {
                cursorTop = Console.GetCursorPosition().Top;
                cursorLeft = Console.GetCursorPosition().Left;
                //Console.SetWindowSize(width, height);
                //if (left < Console.WindowWidth && left < Console.WindowHeight) 
                Console.SetCursorPosition(left, top);
                Debugger.SetCursor();
            }
        }

    }
}
