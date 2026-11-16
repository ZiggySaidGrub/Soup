using System;
using System.Diagnostics;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Soup.GUI;

class Program
{
    private static Sdl2Window window;
    private static GraphicsDevice gd;
    private static CommandList cl;
    private static ImGuiRenderer renderer;

    private static MainWindow mainWindow;

    static void Main(string[] args)
    {
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo(50, 50, 1280, 720, WindowState.Maximized, "Soup"),
            new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
            out window,
            out gd
        );

        window.Resized += () =>
        {
            gd.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
            renderer.WindowResized(window.Width, window.Height);
        };

        cl = gd.ResourceFactory.CreateCommandList();
        renderer = new ImGuiRenderer(gd, gd.MainSwapchain.Framebuffer.OutputDescription, window.Width, window.Height);

        Setup();

        var stopwatch = Stopwatch.StartNew();
        float deltaTime = 0f;

        while (window.Exists && mainWindow.Open)
        {
            deltaTime = stopwatch.ElapsedTicks / (float) Stopwatch.Frequency;
            stopwatch.Restart();

            InputSnapshot snapshot = window.PumpEvents();

            if (!window.Exists || !mainWindow.Open) { break; }

            renderer.Update(deltaTime, snapshot);

            // This is where the ui happens
            SubmitUI();

            cl.Begin();
            cl.SetFramebuffer(gd.MainSwapchain.Framebuffer);
            cl.ClearColorTarget(0, new RgbaFloat(48f/255f, 48f/255f, 48f/255f, 1f));
            renderer.Render(gd, cl);
            cl.End();
            gd.SubmitCommands(cl);
            gd.SwapBuffers(gd.MainSwapchain);
        }

        // Exit
        gd.WaitForIdle();
        renderer.Dispose();
        cl.Dispose();
        gd.Dispose();
    }

    private static void Setup()
    {
        mainWindow = new MainWindow();
    }

    private static void SubmitUI()
    {
        mainWindow.SubmitUI();
    }
}