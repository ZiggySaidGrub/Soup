using ImGuiNET;

namespace Soup.GUI;

public static class MainMenuBar
{
    public static void SubmitUI(MainWindow mainWindow)
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Windows"))
            {
                foreach (DearWindow window in mainWindow.Windows)
                {
                    ImGui.Checkbox(window.Name, ref window.Open);
                }
                
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
    }
}