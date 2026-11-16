namespace Soup.GUI.Windows;

public class BlankNoWindow : DearWindowNoWindow
{
    public override string Name => "Blank No Window";
    public override bool Debug => true;
    public override void SubmitUI() { }
}