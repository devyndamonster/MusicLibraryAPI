using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicPlaylist
    {
        public int Id { get; set; }
        public string PlaylistName { get; set; }
        public List<int> SongIds { get; set; }

    }
}
