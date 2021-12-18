using MusicLibraryAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Services
{
    public interface ICosmosDbService
    {
        Task<UserMusicLibrary> GetUserLibrary(string username);
        Task<MusicArtist> GetArtist(string id);
        Task<List<MusicArtist>> GetArtists(int startIndex, int count);
        Task UpdateArtist(string id, MusicArtist artist);
        Task UpdateUserLibrary(string username, UserMusicLibrary library);
        Task AddArtist(MusicArtist artist);
    }
}
