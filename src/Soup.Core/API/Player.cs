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
            playState = value;
            PlayStateChanged?.Invoke();
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
    public bool Next()
    {
        if (Queue == null) return false;

        Current = Queue.Next();
        if (SongState == SongState.Loading || SongState == SongState.Loaded)
        {
            SongState = SongState.Cleanup;
            Cleanup?.Invoke();
        }
        return true;
    }
    public bool Back()
    {
        if (Queue == null) return false;

        Current = Queue.Back();
        return true;
    }
}