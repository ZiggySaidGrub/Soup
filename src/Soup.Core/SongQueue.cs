using System.Collections.Generic;

namespace Soup.Core;

public class SongQueue
{
    public LinkedList<Song> Queue = new LinkedList<Song>([new Song(), new Song(), new Song(), new Song(), new Song(), new Song()]); // Test Queue
    public LinkedList<Song> BackQueue = [];
    public Song Current;
    public Song Next()
    {
        if (Current != null) BackQueue.AddLast(Current);
        Current = Queue.First.Value;
        Queue.RemoveFirst();
        return Current;
    }
    public bool CanGoNext()
    {
        return Queue.Count > 0;
    }
    public Song Back()
    {
        if (Current != null) Queue.AddFirst(Current);
        Current = BackQueue.Last.Value;
        BackQueue.RemoveLast();
        return Current;
    }
    public bool CanGoBack()
    {
        return BackQueue.Count > 0;
    }
}