using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicPlaylist
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string PlaylistName { get; set; }

        [JsonProperty(PropertyName = "songs")]
        public List<MusicSong> Songs { get; set; }

    }
}
