namespace ConsoleRenderer;

/// OverWrite Cells Render
public class PasserColorChangeSets : Passer {
    public PasserColorChangeSets () {
        name = "OverWrite";
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
                int textColor = arrTextColor[top, left];
                int cellColor = arrCellColor[top, left];

                if (arrText[top, left] != bufferText[top, left] ||
                    //(arrText[top, left] == bufferText[top, left] && prevSymbolWritenToGroupIndex >= 0) || 
                    textColor != bufferTextColor[top, left] ||
                    cellColor != bufferCellColor[top, left]) {
                    /// Check group by colors
                    bool groupFounded = false;
                    for (int iColor = 0; iColor < groups.Count; iColor++) {
                        CellDiffPos group = groups[iColor];
                        if (group.colorText == textColor && group.colorCell == cellColor
                            ) {
                            groupFounded = true;
                            if (prevSymbolWritenToGroupIndex == iColor) {
                                /// Continue text
                                groups[iColor].pos[group.pos.Count-1].text
                                    += Renderer.cellText(top, left);
                            } else {
                                /// Add new Pos
                                groups[iColor].pos.Add(new TextPos(Renderer.cellText(top, left), top, left));
                            }
                            prevSymbolWritenToGroupIndex = iColor;
                            break;
                        }
                    }

                    /// Create group
                    if (!groupFounded) {
                        prevSymbolWritenToGroupIndex = groups.Count;
                        groups.Add(new(textColor, cellColor,
                            new TextPos(Renderer.cellText(top, left), top, left)));
                    }
                } else {
                    prevSymbolWritenToGroupIndex = -1;
                }
            }
        }
    }

}
