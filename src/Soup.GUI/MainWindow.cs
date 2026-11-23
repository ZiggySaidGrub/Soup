using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using ImGuiNET;

namespace Soup.GUI;

public class MainWindow
{
    public Core.Soup Soup;

    public bool Open = true;
    public List<DearWindow> Windows { get; private set; } = [];

    public MainWindow(Core.Soup soup)
    {
        Soup = soup;

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
    public void SubmitUI()
    {
        Tray.SubmitUI(this);

        ImGuiWindowFlags flags = ImGuiWindowFlags.None;
        ImGui.SetNextWindowSize(new Vector2(512, 128), ImGuiCond.FirstUseEver);
        if (ImGui.Begin("Soup", ref Open, flags))
        {
            if (ImGui.Button("back")) { Soup.Player.Back(); };
            ImGui.SameLine(); if (ImGui.Button("pause")) { Soup.Player.PlayState = 1 - Soup.Player.PlayState; }
            ImGui.SameLine(); if (ImGui.Button("next")) { Soup.Player.Next(); };

            ImGui.End();
        }

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