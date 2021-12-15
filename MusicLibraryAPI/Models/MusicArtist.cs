using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class MusicArtist
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public List<int> AlbumIds { get; set; }

    }
}
