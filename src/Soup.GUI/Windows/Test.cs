using ImGuiNET;

namespace Soup.GUI.Windows;

public class Test : DearWindow
{
    public override string Name => "Test";
    public override bool Debug => true;
    public override void SubmitUI()
    {
        ImGui.Text("this is a test window!");
        ImGui.Text("mrrow!");
    }
}