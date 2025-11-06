namespace ConsoleRenderer;

public abstract class Passer {
    public string name = string.Empty;

    public static char[,] arrText;
    public static int[,] arrTextColor;
    public static int[,] arrCellColor;

    public static volatile char[,] bufferText;
    public static int[,] bufferTextColor;
    public static int[,] bufferCellColor;


    public virtual void Init () { }
    public virtual void Pass () { }
    public void Pass_internal () {
        int cellsCount = Renderer.height*Renderer.width;

        Array.Copy(Renderer.arrText, arrText, cellsCount);
        Array.Copy(Renderer.arrTextColor, arrTextColor, cellsCount);
        Array.Copy(Renderer.arrCellColor, arrCellColor, cellsCount);

        Array.Copy(Renderer.bufferText, bufferText, cellsCount);
        Array.Copy(Renderer.bufferTextColor, bufferTextColor, cellsCount);
        Array.Copy(Renderer.bufferCellColor, bufferCellColor, cellsCount);

        Pass();
        //Renderer.SetCursor(0, 0);
    }
}
