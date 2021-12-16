using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class MusicAlbum
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string AlbumName { get; set; }

        [JsonProperty(PropertyName = "artistId")]
        public string ArtistId { get; set; }

        [JsonProperty(PropertyName = "songs")]
        public List<MusicSong> Songs { get; set; }
    }
}
