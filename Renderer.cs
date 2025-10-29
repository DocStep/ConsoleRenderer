using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Linq;
using System.IO;
using System.Threading;


#pragma warning disable CA1416
//#pragma warning disable CS8618
namespace ConsoleRenderer {
    public class Renderer {

        public RendererDebugger debugger;

        public Renderer (int width, int height, Dictionary<int, ConsoleColor> colors) {
            debugger = new RendererDebugger(this);

            this.width = width;
            this.height = height;

            //SetCursor(height-1, width-1);
            //Console.SetWindowSize(width, height);
            //SetColorText(colorText);
            //SetColorCell(colorCell);

            arrText = new char[height, width];
            arrTextColor = new int[height, width];
            arrCellColor = new int[height, width];
            pixelsGs = new float[height, width/2];

            arrText = Simple.ArrayFill(arrText, ' ');
            arrTextColor = Simple.ArrayFill(arrTextColor, colors.ElementAt(0).Key);
            arrCellColor = Simple.ArrayFill(arrCellColor, colors.ElementAt(0).Key);
            //videoCellsGs = Simple.ArrayFill(videoCellsGs, 0);

            bufferText = new char[arrText.GetLength(0), arrText.GetLength(1)];
            bufferTextColor = new int[arrTextColor.GetLength(0), arrTextColor.GetLength(1)];
            bufferCellColor = new int[arrCellColor.GetLength(0), arrCellColor.GetLength(1)];

            this.colors = colors;

            EmptyCanvas(height, width);
        }

        public Renderer (int width, int height) {
            debugger = new RendererDebugger(this);

            this.width = width;
            this.height = height;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);

            arrText = new char[height, width];
            arrTextColor = new int[height, width];
            arrCellColor = new int[height, width];
            pixelsGs = new float[height, width/2];

            arrText = Simple.ArrayFill(arrText, ' ');
            arrTextColor = Simple.ArrayFill(arrTextColor, colors.ElementAt(0).Key);
            arrCellColor = Simple.ArrayFill(arrCellColor, colors.ElementAt(0).Key);
            //videoCellsGs = Simple.ArrayFill(videoCellsGs, 0);

            bufferText = new char[arrText.GetLength(0), arrText.GetLength(1)];
            bufferTextColor = new int[arrTextColor.GetLength(0), arrTextColor.GetLength(1)];
            bufferCellColor = new int[arrCellColor.GetLength(0), arrCellColor.GetLength(1)];

            colors = DefaultValues.colorsBW;

            EmptyCanvas(height, width);
        }


        void InitCanvas (int height, int width) {
            
        }
        void EmptyCanvas (int height, int width) {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = DefaultValues.text;
            Console.BackgroundColor = DefaultValues.cell;
            string text = new string(' ', height*width);
            Console.Write(text);
        }


        public char[,] bufferText;
        public int[,] bufferTextColor;
        public int[,] bufferCellColor;






        public Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>() { { 0, ConsoleColor.Black } };
        public string title = "Renderer Engine";

        public bool isPassing;
        public int height;
        public int width;
        public int iter = 0;
        public bool forceAscii = false;
        public string textAscii = "";


        public char[,] arrText;
        public int[,] arrTextColor;
        public int[,] arrCellColor;





        void ResetLayers () {
            Array.Copy(arrText, bufferText, arrText.Length);
            Array.Copy(arrTextColor, bufferTextColor, arrTextColor.Length);
            Array.Copy(arrCellColor, bufferCellColor, arrCellColor.Length);

            arrText = Simple.ArrayFill(arrText, ' ');
            arrTextColor = Simple.ArrayFill(arrTextColor, colors.ElementAt(0).Key);
            arrCellColor = Simple.ArrayFill(arrCellColor, colors.ElementAt(0).Key);
        }
        public void Pass () {
            isPassing = true;
            iter++;
            debugger.DebugReset();

            if (forceAscii) {
                SetCursor(0, 0);
                Write(textAscii);
            } else {
                if (iter == 0) {
                    CellsFull();
                    //Simple.WriteArray(arrText);
                } else {
                    //CellsFull();
                    CellsOverwrite();
                    //Simple.WriteArray(arrText);
                }
            }


            debugger.DebugOut();

            ResetLayers();
            isPassing = false;
        }



        public Color[,] pixels;
        public float[,] pixelsGs;
        public float blurPower = 0;
        void GetPixels (Bitmap image) {
            pixels = new Color[image.Height, image.Width];
            pixelsGs = new float[image.Height, image.Width];
            for (int top = 0; top < image.Height; top++) {
                for (int left = 0; left < image.Width; left++) {
                    pixels[top, left] = image.GetPixel(left, top);
                    pixelsGs[top, left] = ((pixels[top, left].R * 0.3f) + (pixels[top, left].G * 0.59f) + (pixels[top, left].B * 0.11f))/256f;
                }
            }
        }


        public void VideoToArrays (Bitmap image) {
            image = new Bitmap(image, new Size(width/2, height)); // Resize to fit console window
            GetPixels(image);

            if (blurPower > 0) FrameBlur(image, 0.1f);

            for (int top = 0; top < image.Height; top++) {
                for (int left = 0; left < image.Width; left++) {
                    int idx = (int)(pixelsGs[top, left]*colors.Count);
                    arrCellColor[top, 2*left] = colors.ElementAt(idx).Key;
                    arrCellColor[top, 2*left+1] = colors.ElementAt(idx).Key;

                    //arrText[top, 2*left] = asciiChars[(int)((1-grayScale)*(asciiChars.Length-1))];
                    //arrText[top, 2*left+1] = asciiChars[(int)((1-grayScale)*(asciiChars.Length-1))];

                    //AsciiFromString(top, left, grayScale, 1f/colors.Count);
                    //AsciiFromDictionary(top, left, grayScale, 1f/colors.Count);
                }
            }
        }
        public void VideoToAscii (Bitmap image) {
            image = new Bitmap(image, new Size(width, height)); // Resize to fit console window
            GetPixels(image);

            if (blurPower > 0) FrameBlur(image, blurPower);

            textAscii = "";
            for (int top = 0; top < image.Height; top++) {
                for (int left = 0; left < image.Width; left++) {
                    int idx = (int)(pixelsGs[top, left]*colors.Count);
                    textAscii += asciiChars[asciiChars.Length-1-(int)(pixelsGs[top, left]*asciiChars.Length)];
                }
            }
        }

        void FrameBlur (Bitmap image, float power) {
            //float power = 0.5f;
            for (int top = 0; top < image.Height; top++) {
                for (int left = 0; left < image.Width; left++) {
                    float sum = pixelsGs[top, left] - pixelsGs[top, left]*power;
                    float dev = 1 - power;
                    for (int itop = -1; itop <= 1; itop++) {
                        for (int ileft = -1; ileft <= 1; ileft++) {
                            if (0 <= top+itop && top+itop < image.Height &&
                                0 <= left+ileft && left+ileft < image.Width) {
                                sum += pixelsGs[top+itop, left+ileft]*power;
                                dev += power;
                            }
                        }
                    }
                    pixelsGs[top, left] = sum/dev;
                }
            }
        }


        void AsciiFromString (int top, int left, float grayScale, float treshold) {
            int idx = (int)(grayScale*colors.Count);
            arrCellColor[top, 2*left] = colors.ElementAt(idx).Key;
            arrCellColor[top, 2*left+1] = colors.ElementAt(idx).Key;

            bool asciiUse = true;
            if (asciiUse) {
                if (0.5f-treshold < grayScale && grayScale < 0.5f+treshold) {
                    arrText[top, 2*left] = asciiCharsShort[(int)(grayScale*asciiCharsShort.Length)];
                    arrText[top, 2*left+1] = asciiCharsShort[(int)(grayScale*asciiCharsShort.Length)];
                    if (grayScale > 0) {
                        arrTextColor[top, 2*left] = colors.ElementAt(colors.Count-1-idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(colors.Count-1-idx).Key;
                    } else {
                        arrTextColor[top, 2*left] = colors.ElementAt(idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(idx).Key;
                    }
                } else {
                    arrText[top, 2*left] = ' ';
                    arrText[top, 2*left+1] = ' ';
                }
            }
        }
        void AsciiFromDictionary (int top, int left, float grayScale, float treshold) {
            int idx = (int)(grayScale*colors.Count());
            arrCellColor[top, 2*left] = colors.ElementAt(idx).Key;
            arrCellColor[top, 2*left+1] = colors.ElementAt(idx).Key;

            //float treshold = 1f/colors.Count;
            bool asciiUse = true;

            if (asciiUse) {
                if (0.5f-treshold < grayScale && grayScale < 0.5f+treshold) {
                    float inTresholdScale = (treshold - 0.5f + grayScale)/(2*treshold);
                    int idx_ascii = (int)(inTresholdScale*DefaultValues.videoCellPairChars.Count());

                    arrText[top, 2*left] = DefaultValues.videoCellPairChars.ElementAt(idx_ascii).Key;
                    arrText[top, 2*left+1] = DefaultValues.videoCellPairChars.ElementAt(idx_ascii).Value;

                    if (grayScale > 0.5f) {
                        arrTextColor[top, 2*left] = colors.ElementAt(colors.Count-1-idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(colors.Count-1-idx).Key;
                    } else {
                        arrTextColor[top, 2*left] = colors.ElementAt(idx).Key;
                        arrTextColor[top, 2*left+1] = colors.ElementAt(idx).Key;
                    }
                } else {
                    arrText[top, 2*left] = ' ';
                    arrText[top, 2*left+1] = ' ';
                    arrTextColor[top, 2*left] = colors.ElementAt(idx).Key;
                    arrTextColor[top, 2*left+1] = colors.ElementAt(idx).Key;
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

            while (text.Length > 0) {
                if (left > arrText.GetLength(1)-1) {
                    if (trimEndLine || top >= arrText.GetLength(0)) break;
                    else {
                        left = 0;
                        top++;
                    }
                }
                if (top >= arrText.GetLength(0) || left >= arrText.GetLength(1)) break;

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

                if (line.Length >= 1) {
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



        /* Full Cells Render */
        List<CellDiffPos> groupsToWriteFull;
        void CellsFullGroups () {
            groupsToWriteFull = new List<CellDiffPos>();
            int prevSymbolWritenToGroupIndex = -1;
            for (int top = 0; top < arrCellColor.GetLength(0); top++)
                for (int left = 0; left < arrCellColor.GetLength(1); left++) {
                    // check group by color
                    bool groupFound = false;
                    for (int iColor = 0; iColor < groupsToWriteFull.Count; iColor++) {
                        if (arrTextColor[top, left] == groupsToWriteFull[iColor].colorText && 
                            arrCellColor[top, left] == groupsToWriteFull[iColor].colorCell) {
                            groupFound = true;
                            if (prevSymbolWritenToGroupIndex == iColor) {
                                groupsToWriteFull[iColor].pos[groupsToWriteFull[iColor].pos.Count-1].text += $"{arrText[top, left], 1}";
                            } else {
                                groupsToWriteFull[iColor].pos.Add(new Pos($"{arrText[top, left], 1}", top, left));
                            }
                            prevSymbolWritenToGroupIndex = iColor;
                            break;
                        }
                    }

                    if (!groupFound) {
                        prevSymbolWritenToGroupIndex = groupsToWriteFull.Count;
                        groupsToWriteFull.Add(new(arrTextColor[top, left], arrCellColor[top, left],
                            new Pos($"{arrText[top, left], 1}", top, left)));
                    }
                }
        }
        public void CellsFull () {
            CellsFullGroups();

            for (int iColor = 0; iColor < groupsToWriteFull.Count; iColor++) {
                SetColorText(groupsToWriteFull[iColor].colorText);
                SetColorCell(groupsToWriteFull[iColor].colorCell);
                for (int iText = 0; iText < groupsToWriteFull[iColor].pos.Count; iText++) {
                    CellFullDraw(groupsToWriteFull[iColor].pos[iText].text, groupsToWriteFull[iColor].pos[iText].top, groupsToWriteFull[iColor].pos[iText].left);
                }
            }
        }
        void CellFullDraw (string text, int startTop, int startLeft) {
            SetCursor(startTop, startLeft);
            Write(text);
        }



        /* OverWrite Cells Render */
        List<CellDiffPos> groupsToWriteOver;
        void CellsOverwriteGroups () {
            groupsToWriteOver = new List<CellDiffPos>();
            int prevSymbolWritenToGroupIndex = -1;
            for (int top = 0; top < arrCellColor.GetLength(0); top++)
                for (int left = 0; left < arrCellColor.GetLength(1); left++) {
                    if (arrText[top, left] != bufferText[top, left] ||
                        //(arrText[top, left] == bufferText[top, left] && prevSymbolWritenToGroupIndex >= 0) || 
                        arrTextColor[top, left] != bufferTextColor[top, left] ||
                        arrCellColor[top, left] != bufferCellColor[top, left]) {
                        // check group by colors
                        bool groupFounded = false;
                        for (int iColor = 0; iColor < groupsToWriteOver.Count; iColor++) {
                            if (groupsToWriteOver[iColor].colorText == arrTextColor[top, left] &&
                                groupsToWriteOver[iColor].colorCell == arrCellColor[top, left]) {
                                groupFounded = true;
                                if (prevSymbolWritenToGroupIndex == iColor) {
                                    groupsToWriteOver[iColor].pos[groupsToWriteOver[iColor].pos.Count-1].text
                                        += $"{arrText[top, left], 1}";
                                } else {
                                    groupsToWriteOver[iColor].pos.Add(new Pos($"{arrText[top, left], 1}", top, left));
                                }
                                prevSymbolWritenToGroupIndex = iColor;
                                break;
                            }
                        }

                        // create group
                        if (!groupFounded) {
                            prevSymbolWritenToGroupIndex = groupsToWriteOver.Count;
                            groupsToWriteOver.Add(new(arrTextColor[top, left], arrCellColor[top, left],
                                new Pos($"{arrText[top, left], 1}", top, left)));
                        }
                    } else {
                        prevSymbolWritenToGroupIndex = -1;
                    }
                }
        }
        public void CellsOverwrite () {
            CellsOverwriteGroups();

            for (int iColor = 0; iColor < groupsToWriteOver.Count; iColor++) {
                SetColorText(groupsToWriteOver[iColor].colorText);
                SetColorCell(groupsToWriteOver[iColor].colorCell);
                for (int iText = 0; iText < groupsToWriteOver[iColor].pos.Count; iText++) {
                    CellOverwriteDraw(groupsToWriteOver[iColor].pos[iText].text, 
                        groupsToWriteOver[iColor].pos[iText].top, groupsToWriteOver[iColor].pos[iText].left);
                }
            }
        }
        void CellOverwriteDraw (string text, int startTop, int startLeft) {
            SetCursor(startTop, startLeft);
            Write(text);
        }



        /* Simple */
        int colorCell = 0;
        int colorText = 0;
        int cursorTop = 0;
        int cursorLeft = 0;
        public void Write (string s) {
            Console.Write(s);
            debugger.Write();
        }
        public void WriteLine (string s) {
            Console.WriteLine(s);
            debugger.Write();
        }
        public void SetColorCell (ConsoleColor colorCell) {
            int colorKey = colors.First(x => x.Value == colorCell).Key;
            if (this.colorCell != colorKey) {
                this.colorCell = colorKey;
                Console.BackgroundColor = colorCell;
                debugger.SetColorCell();
            }
        }
        public void SetColorCell (int colorCell) {
            if (this.colorCell != colorCell) {
                this.colorCell = colorCell;
                Console.BackgroundColor = colors.First(x => x.Key == colorCell).Value;
                //Console.BackgroundColor = colors[colorCell];
                debugger.SetColorCell();
            }
        }
        // add "space" string check
        public void SetColorText (ConsoleColor colorText) {
            int colorKey = colors.First(x => x.Value == colorText).Key;
            if (this.colorText != colorKey) {
                this.colorText = colorKey;
                Console.ForegroundColor = colorText;
                debugger.SetColorText();
            }
        }
        public void SetColorText (int colorText) {
            if (this.colorText != colorText) {
                this.colorText = colorText;
                Console.ForegroundColor = colors.First(x => x.Key == colorText).Value;
                debugger.SetColorText();
            }
        }
        public void SetCursor (int top, int left) {
            if (top != Console.GetCursorPosition().Top || left != Console.GetCursorPosition().Left) {
                cursorTop = Console.GetCursorPosition().Top;
                cursorLeft = Console.GetCursorPosition().Left;
                //Console.SetWindowSize(width, height);
                //if (left < Console.WindowWidth && left < Console.WindowHeight) 
                Console.SetCursorPosition(left, top);
                debugger.SetCursor();
            }
        }

    }
}
