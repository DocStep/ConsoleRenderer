using System;

namespace ConsoleRenderer {
    public class Legacy {

        /* OverWrite Cells Render */
        /*public void OverWriteCells () {
            //List<List<int[]>> cellsColorsToOverride = new();
            //for (int i = 0; i < colors.Count; i++) cellsColorsToOverride.Add(new List<int[]>());

            List<int[]>[] cellsColorsToOverride = new List<int[]>[3] { new List<int[]>(), new List<int[]>(), new List<int[]>() };
            for (int top = 0; top < arrCells.GetLength(0); top++)
                for (int left = 0; left < arrCells.GetLength(1); left++)
                    if (arrCells[top, left] != bufferCells[top, left]) cellsColorsToOverride[arrCells[top, left]].Add(new int[2] { top, left });

            for (int iColor = 0; iColor < cellsColorsToOverride.Length; iColor++) {
                if (cellsColorsToOverride[iColor].Count > 0) {
                    SetColorCell(iColor);
                    string part = "";
                    int startTop = 0, startLeft = 0, lastLeft = 0, lastTop = 0;
                    for (int cell = 0; cell < cellsColorsToOverride[iColor].Count; cell++) {
                        int[] cords = cellsColorsToOverride[iColor][cell];
                        //if (startLine == cords[0] && lastX+1 == cords[1]) part += $"  ";
                        if (lastLeft+1 == cords[1] &&
                            (startTop == cords[0] || (cords[0] == lastTop+1 && cords[1] == arrCells.GetLength(1)-1))) part += $"  ";
                        else {
                            if (part.Length > 0) OverWriteCell(part, startTop, startLeft);
                            startTop = cords[0];
                            startLeft = cords[1];
                            part = $"  ";
                        }
                        lastLeft = cords[1];
                        lastTop = cords[0];
                    }
                    OverWriteCell(part, startTop, startLeft);
                }
            }
        }
        void OverWriteCell (string text, int startTop, int startLeft) {
            SetCursor(startTop, 2*startLeft);
            Write(text);
            //Thread.Sleep(1);
        }*/

        /* OverWrite Texts Render */
        /*public void OverWriteTexts () {
            TextsGroups();

            string part = "";
            int lastColorText = arrTextsColor[0, 0];
            int lastColorCell = arrTextsCellColor[0, 0];
            int startTop = 0, startLeft = 0, startColorText = 0, startColorCell = 0;
            for (int top = 0; top < arrTexts.GetLength(0); top++) {
                for (int left = 0; left < arrTexts.GetLength(1); left++) {
                    if (arrTexts[top, left] == bufferTexts[top, left] &&
                        arrTextsColor[top, left] == bufferTextsColor[top, left] &&
                        arrTextsCellColor[top, left] == bufferTextsCellColor[top, left] &&
                        lastColorText == arrTextsColor[top, left] && lastColorCell == arrTextsCellColor[top, left]
                        ) {
                        //part += arrTexts[top, left];
                    } else {
                        if (part.Length > 0) OverWriteText(arrTexts[top, left], startTop, startLeft, startColorText, startColorCell);
                        part = arrTexts[top, left];
                        startTop = top;
                        startLeft = left;
                        startColorText = arrTextsColor[startTop, startLeft];
                        startColorCell = arrTextsCellColor[startTop, startLeft];
                    }
                    lastColorText = arrTextsColor[top, left];
                    lastColorCell = arrTextsCellColor[top, left];
                }
            }
            if (part.Length > 0) OverWriteText(part, startTop, startLeft, startColorText, startColorCell);
        }
        void OverWriteText (string text, int top, int left, int colorText, int colorCell) {
            SetCursor(top, left);
            SetColorText(colorText);
            SetColorCell(colorCell);
            Write(text);

            //Thread.Sleep(00);
        }*/


        

        ///* 0 */
        /*public void DrawCells () {
            statsReset();
            Console.Clear();
            for (int y = 0; y < arr.GetLength(0); y++) {
                SetCursor(0, y);
                for (int x = 0; x < arr.GetLength(1); x++) {
                    SetColor(arr[y, x]);
                    Write("  ");
                }
            }
            statsOut();
        }*/
        //
        ///* 1 */
        /*public void DrawStrings () {
            statsReset();
            Console.Clear();
            for (int y = 0; y < arr.GetLength(0); y++) {
                string part = "";
                int last = -1;
                int lastColor = -1;
                SetCursor(0, y);
                for (int x = 0; x < arr.GetLength(1); x++) {
                    if (last == arr[y, x]) {
                        part += "  ";
                    } else {
                        if (part.Length > 0) DrawString(part, lastColor);
                        last = arr[y, x];
                        part = "  ";
                        lastColor = arr[y, x];
                    }
                }
                DrawString(part, lastColor);
            }
            statsOut();
        }
        void DrawString (string value, int color) {
            SetColor(color);
            Write(value);
            Thread.Sleep(delay);
        }*/
        
        ///* 3 */
        /*public void OverwriteCells () {
            statsReset();
            int[,] diff = new int[arr.GetLength(0), arr.GetLength(1)];
            for (int y = 0; y < arr.GetLength(0); y++)
                for (int x = 0; x < arr.GetLength(1); x++) {
                    diff[y, x] = -1000;
                    if (buffer[y, x] != arr[y, x]) diff[y, x] = arr[y, x];
                }
        
            for (int i = 0; i < arr.GetLength(0); i++) {
                for (int f = 0; f < arr.GetLength(1); f++) {
                    if (diff[i, f] > -1) {
                        SetColor(diff[i, f]);
                        SetCursor(f*2, i);
                        Write($"  ");
                    }
                }
            }
            statsOut();
        }*/
        
        ///* 4 */
        /*public void OverwriteCellsOptimized () {
            statsReset();
            List<int[]>[] cellsToOverride = new List<int[]>[3] { new List<int[]>(), new List<int[]>(), new List<int[]>() };
            for (int y = 0; y < arr.GetLength(0); y++)
                for (int x = 0; x < arr.GetLength(1); x++)
                    if (buffer[y, x] != arr[y, x]) cellsToOverride[arr[y, x]].Add(new int[2] { y, x });
        
            for (int cell = 0; cell < cellsToOverride.Length; cell++) {
                SetColor(cell);
                for (int l = 0; l < cellsToOverride[cell].Count; l++) {
                    SetCursor(cellsToOverride[cell][l][1]*2, cellsToOverride[cell][l][0]);
                    Write("  ");
                }
            }
            statsOut();
        }*/

    }
}
