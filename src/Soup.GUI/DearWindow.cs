namespace Soup.GUI;

public abstract class DearWindow
{
    public bool Open = false;

    public abstract string Name { get; }
    public virtual bool Debug => false;
    public abstract void SubmitUI();
}

public abstract class DearWindowNoWindow : DearWindow { }