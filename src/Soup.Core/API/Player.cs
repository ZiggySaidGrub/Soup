using System;

namespace Soup.Core.API;

public enum PlayState
{
    Playing,
    Paused
}
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

    public SongQueue Queue;
    public Song Current;
    public event Action<Song> NewSong;
    public bool Next()
    {
        if (Queue == null) return false;
        
        Current = Queue.Next();
        NewSong?.Invoke(Current);
        return true;
    }
    public bool Back()
    {
        if (Queue == null) return false;

        Current = Queue.Back();
        NewSong?.Invoke(Current);
        return true;
    }
}