using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using ImGuiNET;

namespace Soup.GUI;

public class MainWindow
{
    public bool Open = true;

    private List<DearWindow> windows = [];

    public MainWindow()
    {
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
            if (!t.IsAbstract && typeof(DearWindow).IsAssignableFrom(t))
            {
                DearWindow win = (DearWindow) Activator.CreateInstance(t);
                windows.Add(win);
            }
        }
        windows.Sort(new Comparison<DearWindow>((x, y) => string.Compare(x.Name, y.Name)));
    }
    public void SubmitUI()
    {
        ImGuiWindowFlags flags = ImGuiWindowFlags.MenuBar;

        ImGui.SetNextWindowSize(new Vector2(512, 128), ImGuiCond.FirstUseEver);
        if (ImGui.Begin("Soup", ref Open, flags))
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Windows"))
                {
                    foreach (DearWindow window in windows)
                    {
                        ImGui.Checkbox(window.Name, ref window.Open);
                    }
                    
                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }

            ImGui.Text("meow");

            ImGui.End();
        }

        foreach (DearWindow window in windows)
        {
            if (window.Open && ImGui.Begin(window.Name, ref window.Open))
            {
                window.SubmitUI();
                ImGui.End();
            }
        }
    }
}