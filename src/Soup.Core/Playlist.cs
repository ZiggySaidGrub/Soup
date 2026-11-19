using System.Collections.Generic;

namespace Soup.Core;

public class Playlist
{
    public string Name { get; private set; }
    public List<Song> Songs { get; private set; }
}