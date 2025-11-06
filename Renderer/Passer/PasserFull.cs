namespace ConsoleRenderer;

/// Full Cells Render
public class PasserFull : Passer {
    public PasserFull () {
        name = "Full";
    }

    public override void Pass () {
        Convert();
        Renderer.WriteChangeSet(groups);
    }


    List<CellDiffPos> groups = new List<CellDiffPos>();

    void Convert () {
        groups.Clear();
        int prevSymbolWritenToGroupIndex = -1;
        for (int top = 0; top < Renderer.height; top++) {
            if (Renderer.Interruption(top)) return;

            for (int left = 0; left < Renderer.width; left++) {
                /// Check group by color
                bool groupFound = false;
                int textColor = arrTextColor[top, left];
                int cellColor = arrCellColor[top, left];
                for (int iColor = 0; iColor < groups.Count; iColor++) {
                    CellDiffPos group = groups[iColor];
                    if (textColor == group.colorText && cellColor == group.colorCell) {
                        groupFound = true;
                        if (prevSymbolWritenToGroupIndex == iColor) {
                            groups[iColor].pos[group.pos.Count-1].text += Renderer.cellText(top, left);
                        } else {
                            groups[iColor].pos.Add(new TextPos(Renderer.cellText(top, left), top, left));
                        }
                        prevSymbolWritenToGroupIndex = iColor;
                        break;
                    }
                }

                if (!groupFound) {
                    prevSymbolWritenToGroupIndex = groups.Count;
                    groups.Add(new(textColor, cellColor, new TextPos(Renderer.cellText(top, left), top, left)));
                }
            }
        }
        Renderer.cellsToChangeCount = Renderer.height*Renderer.width;
    }
}
