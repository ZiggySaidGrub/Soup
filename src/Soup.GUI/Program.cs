using System.Diagnostics;
using System.Runtime.CompilerServices;
using Soup.GUI.Audio;
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

    private static WindowManager windowManager;
    private static AudioManager audioManager;
    private static Core.Soup soup;

    static void Main(string[] args)
    {
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo(50, 50, 1280, 720, WindowState.Maximized, "Soup"),
            new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
            out window,
            out gd
        );

        ApplicationIcon.SetWindowIcon(window.SdlWindowHandle, "Assets/icon512.bmp");

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

        while (window.Exists && windowManager.MainWindow.Open)
        {
            deltaTime = stopwatch.ElapsedTicks / (float) Stopwatch.Frequency;
            stopwatch.Restart();

            InputSnapshot snapshot = window.PumpEvents();

            if (!window.Exists || !windowManager.MainWindow.Open) { break; }

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
        Cleanup();
        gd.WaitForIdle();
        renderer.Dispose();
        cl.Dispose();
        gd.Dispose();
    }

    private static void Setup()
    {
        soup = new Core.Soup();

        audioManager = new AudioManager(soup);
        windowManager = new WindowManager(soup);
    }
    private static void Cleanup()
    {
        audioManager.Dispose();
    }

    private static void SubmitUI()
    {
        windowManager.DrawWindows();
    }
}