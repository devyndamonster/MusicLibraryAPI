using MusicLibraryAPI.Models;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Services
{
    public interface ICosmosDbService
    {
        Task<UserMusicLibrary> GetUserLibrary(string username);
        Task<MusicArtist> GetArtist(int id);
        Task UpdateArtist(int id, MusicArtist artist);
        Task UpdateUserLibrary(string username, UserMusicLibrary library);
    }
}
