namespace ConsoleRenderer;

/// Full Cells Render
public class PasserFull : Passer {
    public PasserFull () {
        name = "Full";
    }

    public override void Pass () {
        CellsFullGroups();
        Renderer.WriteChangeSet(groupsToWriteFull);
    }

    List<CellDiffPos> groupsToWriteFull = new List<CellDiffPos>();
    void CellsFullGroups () {
        groupsToWriteFull.Clear();
        int prevSymbolWritenToGroupIndex = -1;
        for (int top = 0; top < Renderer.height; top++) {
            if (Renderer.shouldInterrupt) {
                Renderer.shouldInterrupt = false;
                RendererDebugger.framesSkipped++;
                Renderer.matrixSize = top*Renderer.width;
                return;
            }

            for (int left = 0; left < Renderer.width; left++) {
                /// Check group by color
                bool groupFound = false;
                int textColor = Renderer.arrTextColor[top, left];
                int cellColor = Renderer.arrCellColor[top, left];
                for (int iColor = 0; iColor < groupsToWriteFull.Count; iColor++) {
                    CellDiffPos group = groupsToWriteFull[iColor];
                    if (textColor == group.colorText && cellColor == group.colorCell) {
                        groupFound = true;
                        if (prevSymbolWritenToGroupIndex == iColor) {
                            groupsToWriteFull[iColor].pos[group.pos.Count-1].text += Renderer.cellText(top, left);
                        } else {
                            groupsToWriteFull[iColor].pos.Add(new TextPos(Renderer.cellText(top, left), top, left));
                        }
                        prevSymbolWritenToGroupIndex = iColor;
                        break;
                    }
                }

                if (!groupFound) {
                    prevSymbolWritenToGroupIndex = groupsToWriteFull.Count;
                    groupsToWriteFull.Add(new(textColor, cellColor, new TextPos(Renderer.cellText(top, left), top, left)));
                }
            }
        }
        Renderer.matrixSize = Renderer.height*Renderer.width;
    }
}
