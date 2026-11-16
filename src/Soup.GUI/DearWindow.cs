namespace Soup.GUI;

public abstract class DearWindow
{
    public bool Open = false;

    public abstract string Name { get; }
    public abstract void SubmitUI();
}