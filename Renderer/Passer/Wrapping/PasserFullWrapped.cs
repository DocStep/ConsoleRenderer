namespace ConsoleRenderer;

/// Full Cells Render
public class PasserFullWrapped : Passer {
    public PasserFullWrapped () {
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

            int textColor = arrTextColor[top, 0];
            int cellColor = arrCellColor[top, 0];
            prevSymbolWritenToGroupIndex = groups.Count;
            groups.Add(new(textColor, cellColor, new TextPos(Renderer.cellText(top, 0), top, 0)));

            for (int left = 1; left < Renderer.width; left++) {
                /// Check group by color
                bool groupFound = false;
                textColor = arrTextColor[top, left];
                cellColor = arrCellColor[top, left];
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
