using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicLibrary
    {

        public string UserName { get; set; }
        public List<int> LikedArtistIds { get; set; }
        public List<MusicAlbum> LikedAlbums { get; set; }
        public List<MusicSong> LikedSongs { get; set; }
        public List<UserMusicPlaylist> Playlists { get; set; }
           

    }
}
