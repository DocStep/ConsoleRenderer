namespace ConsoleRenderer;

/// Full Cells Render
public class PasserColorSort : Passer {
    public PasserColorSort () {
        name = "ColorSort";
    }

    public override void Pass () {
        Convert();

        RendererSimple.FillWrapping(Renderer.colors[i_mostColor]);
        Renderer.WriteChangeSet(groups);
    }


    List<CellDiffPos> groups = new List<CellDiffPos>();
    int i_mostColor = -1;

    void Convert () {
        groups.Clear();
        i_mostColor = RendererSimple.i_GetCellsMostColor();
        //return;

        for (int top = 0; top < Renderer.height; top++) {
            if (Renderer.Interruption(top)) return;

            int iColorLast = -1;
            for (int left = 0; left < Renderer.width; left++) {
                /// Check group by color
                bool groupFound = false;
                int textColor = arrTextColor[top, left];
                int cellColor = arrCellColor[top, left];
                if (cellColor != i_mostColor) {
                    for (int iColor = 0; iColor < groups.Count; iColor++) {
                        CellDiffPos group = groups[iColor];
                        if (cellColor == group.colorCell) {
                            groupFound = true;
                            /// Check Text continuing
                            if (iColorLast == iColor) {
                                groups[iColor].pos[group.pos.Count-1].text += Renderer.cellText(top, left);
                            } else {
                                groups[iColor].pos.Add(new TextPos(Renderer.cellText(top, left), top, left));
                            }

                            iColorLast = iColor;
                            break;
                        }
                    }

                    if (!groupFound) {
                        iColorLast = groups.Count;
                        groups.Add(new(textColor, cellColor, new TextPos(Renderer.cellText(top, left), top, left)));
                    }
                } else {
                    iColorLast = -1;
                }
            }
        }

        Renderer.cellsToChangeCount = Renderer.height*Renderer.width;

        //groups.Remove(i_mostColor);
    }

}
