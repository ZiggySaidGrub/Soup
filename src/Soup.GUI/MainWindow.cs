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
    public WindowManager WindowManager;

    public MainWindow(Core.Soup soup)
    {
        Soup = soup;
    }

    private float localTime = 0f;
    public void SubmitUI()
    {
        ImGuiWindowFlags flags = ImGuiWindowFlags.None;
        ImGui.SetNextWindowSize(new Vector2(512, 128), ImGuiCond.FirstUseEver);
        if (!ImGui.Begin("Soup", ref Open, flags)) return;
        
        if (ImGui.Button("back")) { Soup.Player.Back(); };
        ImGui.SameLine(); if (ImGui.Button("pause")) { Soup.Player.PlayState = 1 - Soup.Player.PlayState; }
        ImGui.SameLine(); if (ImGui.Button("next")) { Soup.Player.Next(); };
        
        var totalTime = Soup.Player.GetTotalTime();
        float totalTimeSeconds;
        if (totalTime == null) { totalTimeSeconds = 0f; } else { totalTimeSeconds = (float) totalTime.Value.TotalSeconds; }
        
        TimeSpan localTimeSpan = TimeSpan.FromSeconds(localTime);

        string sliderText = $"{localTimeSpan:m\\:ss}";
        string sliderLabel = $"{totalTime:m\\:ss}";
        
        if (totalTime == null) {
            sliderLabel = "-:--";
            sliderText = "-:--";
        }

        var labelWidth = ImGui.CalcTextSize(sliderLabel).X;
        var spacing = ImGui.GetStyle().ItemInnerSpacing.X;
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - labelWidth - spacing);

        ImGui.SliderFloat(sliderLabel, ref localTime, 0f, totalTimeSeconds, sliderText);
        
        if (ImGui.IsItemDeactivated())
        {
            Soup.Player.SetPlaybackTime(TimeSpan.FromSeconds(localTime));
        }
        else if (!ImGui.IsItemActive())
        {
            var playbackTime = Soup.Player.GetPlaybackTime();
            if (playbackTime == null) { localTime = 0f; } else { localTime = (float) playbackTime.Value.TotalSeconds; }
        }

        ImGui.End();
    }
}