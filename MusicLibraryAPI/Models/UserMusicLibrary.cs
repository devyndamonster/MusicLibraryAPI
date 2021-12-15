using System.Collections.Generic;

namespace MusicLibraryAPI.Models
{
    public class UserMusicLibrary
    {

        public string UserName { get; set; }
        public List<int> LikedAlbumIds { get; set; }
        public List<int> LikedArtistIds { get; set; }
        public List<int> LikedSongIds { get; set; }
        public List<UserMusicPlaylist> Playlists { get; set; }
           

    }
}
