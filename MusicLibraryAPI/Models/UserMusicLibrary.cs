using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicLibrary
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "likedArtistIds")]
        public List<int> LikedArtistIds { get; set; }

        [JsonProperty(PropertyName = "likedArtistIds")]
        public List<MusicAlbum> LikedAlbums { get; set; }

        [JsonProperty(PropertyName = "likedSongs")]
        public List<MusicSong> LikedSongs { get; set; }

        [JsonProperty(PropertyName = "playlists")]
        public List<UserMusicPlaylist> Playlists { get; set; }
           

    }
}
