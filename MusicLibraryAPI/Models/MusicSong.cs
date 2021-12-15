namespace MusicLibraryAPI.Models
{
    public class MusicSong
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }

    }
}
