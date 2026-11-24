using System;
using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;

namespace Soup.GUI;

public class WindowManager
{
    public Core.Soup Soup;

    public List<DearWindow> Windows { get; private set; } = [];
    public MainWindow MainWindow;
    public WindowManager(Core.Soup soup)
    {
        Soup = soup;

        MainWindow = new MainWindow(Soup);

        // Init Windows
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
            if (!t.IsAbstract && typeof(DearWindow).IsAssignableFrom(t))
            {
                DearWindow win = (DearWindow) Activator.CreateInstance(t);
            #if !DEBUG
                if (win.Debug) continue;
            #endif

                Windows.Add(win);
            }
        }
        Windows.Sort(new Comparison<DearWindow>((x, y) => string.Compare(x.Name, y.Name)));
    }

    public void DrawWindows()
    {
        Tray.SubmitUI(this);

        MainWindow.SubmitUI();

        foreach (DearWindow window in Windows)
        {
            if (!window.Open) continue;

            if (window is DearWindowNoWindow)
            {
                window.SubmitUI();
                continue;
            }

            if (ImGui.Begin(window.Name, ref window.Open))
            {
                window.SubmitUI();
                ImGui.End();
            }
        }
    }
}