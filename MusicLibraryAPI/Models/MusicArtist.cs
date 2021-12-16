using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class MusicArtist
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "artistName")]
        public string ArtistName { get; set; }

        [JsonProperty(PropertyName = "albums")]
        public List<MusicAlbum> Albums { get; set; }

    }
}
