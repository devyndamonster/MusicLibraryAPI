using Newtonsoft.Json;

namespace MusicLibraryAPI.Models
{
    public class MusicSong
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string SongName { get; set; }

        [JsonProperty(PropertyName = "artistId")]
        public string ArtistId { get; set; }

        [JsonProperty(PropertyName = "artistName")]
        public string ArtistName { get; set; }

        [JsonProperty(PropertyName = "albumId")]
        public string AlbumId { get; set; }

        [JsonProperty(PropertyName = "albumName")]
        public string AlbumName { get; set; }
    }
}
