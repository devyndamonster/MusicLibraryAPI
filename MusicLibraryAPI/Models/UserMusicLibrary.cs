using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicLibrary
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "likedArtistIds")]
        public List<string> LikedArtistIds { get; set; }

        [JsonProperty(PropertyName = "likedArtistIds")]
        public List<MusicAlbum> LikedAlbums { get; set; }

        [JsonProperty(PropertyName = "likedSongs")]
        public List<MusicSong> LikedSongs { get; set; }

        [JsonProperty(PropertyName = "playlists")]
        public List<UserMusicPlaylist> Playlists { get; set; }


        //Default constructor for deserializing
        public UserMusicLibrary() { }

        public UserMusicLibrary(string username)
        {
            UserName = username;
            LikedArtistIds = new List<string>();
            LikedAlbums = new List<MusicAlbum>();
            LikedSongs = new List<MusicSong>();
            Playlists = new List<UserMusicPlaylist>();
        }

    }
}
