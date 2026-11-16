using ImGuiNET;

namespace Soup.GUI.Windows;

public class Demo : DearWindowNoWindow
{
    public override string Name => "Demo";
    public override bool Debug => true;
    public override void SubmitUI()
    {
        ImGui.ShowDemoWindow(ref Open);
    }
}