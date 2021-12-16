using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class MusicArtist
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string ArtistName { get; set; }

        [JsonProperty(PropertyName = "albums")]
        public List<MusicAlbum> Albums { get; set; }

    }
}
