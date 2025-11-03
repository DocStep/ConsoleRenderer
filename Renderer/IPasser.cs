namespace ConsoleRenderer;

public abstract class Passer {
    public string name = string.Empty;

    public virtual void Init () { }
    public virtual void Pass () { }
}
