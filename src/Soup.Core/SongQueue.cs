using System.Collections.Generic;

namespace Soup.Core;

public class SongQueue
{
    public LinkedList<Song> Queue;
    public LinkedList<Song> BackQueue;
    public Song Current;
    public Song Next()
    {
        BackQueue.AddLast(Current);
        Current = Queue.First.Value;
        Queue.RemoveFirst();
        return Current;
    }
    public Song Back()
    {
        Queue.AddFirst(Current);
        Current = BackQueue.Last.Value;
        BackQueue.RemoveLast();
        return Current;
    }
}