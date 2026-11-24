using System.Numerics;
using ImGuiNET;

namespace Soup.GUI;

public static class Tray
{
    public static void SubmitUI(WindowManager windowManager)
    {
        SubmitMainMenuBar(windowManager);
    }

    private static void SubmitMainMenuBar(WindowManager windowManager)
    {
        ImGui.PushStyleColor(ImGuiCol.MenuBarBg, Vector4.Zero);
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Windows"))
            {
                foreach (DearWindow window in windowManager.Windows)
                {
                    // ImGui.Checkbox(window.Name, ref window.Open);
                    ImGui.MenuItem(window.Name, null, ref window.Open);
                }
                
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
        ImGui.PopStyleColor();
    }
}