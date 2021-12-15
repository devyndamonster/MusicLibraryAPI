using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class MusicAlbum
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public int ArtistId { get; set; }
        public List<int> SongIds { get; set; }
    }
}
