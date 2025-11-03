using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderer {
    /// OverWrite Cells Render
    public class PasserColorChangeSets : Passer {
        public PasserColorChangeSets () {
            name = "OverWrite";
        }

        public override void Pass () {
            CellsOverwriteGroups();
            Renderer.WriteChangeSet(groupsToWriteOver);
        }

        static List<CellDiffPos> groupsToWriteOver = new List<CellDiffPos>();


        static void CellsOverwriteGroups () {
            groupsToWriteOver.Clear();
            int prevSymbolWritenToGroupIndex = -1;
            for (int top = 0; top < Renderer.height; top++) {
                /// Interrupt Check
                if (Renderer.shouldInterrupt) {
                    Renderer.shouldInterrupt = false;
                    RendererDebugger.framesSkipped++;
                    return;
                }
                for (int left = 0; left < Renderer.width; left++) {
                    int textColor = Renderer.arrTextColor[top, left];
                    int cellColor = Renderer.arrCellColor[top, left];

                    if (Renderer.arrText[top, left] != Renderer.bufferText[top, left] ||
                        //(arrText[top, left] == bufferText[top, left] && prevSymbolWritenToGroupIndex >= 0) || 
                        textColor != Renderer.bufferTextColor[top, left] ||
                        cellColor != Renderer.bufferCellColor[top, left]) {
                        // check group by colors
                        bool groupFounded = false;
                        for (int iColor = 0; iColor < groupsToWriteOver.Count; iColor++) {
                            CellDiffPos group = groupsToWriteOver[iColor];
                            if (group.colorText == textColor && group.colorCell == cellColor
                                ) {
                                groupFounded = true;
                                if (prevSymbolWritenToGroupIndex == iColor) {
                                    groupsToWriteOver[iColor].pos[group.pos.Count-1].text
                                        += Renderer.cellText(top, left);
                                } else {
                                    groupsToWriteOver[iColor].pos.Add(new Pos(Renderer.cellText(top, left), top, left));
                                }
                                prevSymbolWritenToGroupIndex = iColor;
                                break;
                            }
                        }

                        // create group
                        if (!groupFounded) {
                            prevSymbolWritenToGroupIndex = groupsToWriteOver.Count;
                            groupsToWriteOver.Add(new(textColor, cellColor,
                                new Pos(Renderer.cellText(top, left), top, left)));
                        }
                    } else {
                        prevSymbolWritenToGroupIndex = -1;
                    }
                }
            }
        }
    }
}
