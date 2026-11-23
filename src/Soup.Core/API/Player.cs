using System;

namespace Soup.Core.API;

public enum PlayState
{
    Playing,
    Paused
}
public enum SongState
{
    Cleanup,
    Loading,
    Loaded
}

// TODO: This class can probably be improved a lot
// like in theory everything here works, but it just feels wrong to me
public class Player
{
    internal Player() { }

    public event Action PlayStateChanged;
    private PlayState playState;
    public PlayState PlayState
    {
        get
        {
            return playState;
        }
        set
        {
            var oldState = playState;
            playState = value;
            if (oldState != playState) PlayStateChanged?.Invoke();
        }
    }

    private SongState songState = SongState.Loaded;
    public SongState SongState
    {
        get
        {
            return songState;
        }
        set
        {
            Console.WriteLine($"song state being set to {value}");
            songState = value;
        }
    }

    public SongQueue Queue = new SongQueue();
    public Song Current;
    public event Action Cleanup;
    public event Action<Song> LoadNew;
    public void ReadyForLoad()
    {
        if (SongState == SongState.Cleanup)
        {
            SongState = SongState.Loading;
            LoadNew?.Invoke(Current);
        }
    }
    public void SongOver()
    {
        Next();
    }
    public bool Next()
    {
        if (Queue == null) return false;
        if (!Queue.CanGoNext()) return false;

        Current = Queue.Next();
        StartCleanup();
        return true;
    }
    public bool Back()
    {
        if (Queue == null) return false;
        if (!Queue.CanGoBack()) return false;

        Current = Queue.Back();
        StartCleanup();
        return true;
    }
    private void StartCleanup()
    {
        if (SongState == SongState.Loading || SongState == SongState.Loaded)
        {
            SongState = SongState.Cleanup;
            Cleanup?.Invoke();
        }
    }
}