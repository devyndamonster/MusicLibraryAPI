using Microsoft.Azure.Cosmos;
using MusicLibraryAPI.Models;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Services
{


    public class CosmosDbService : ICosmosDbService
    {
        private Container _userContainer;
        private Container _artistContainer;

        public Task<MusicArtist> GetArtist(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserMusicLibrary> GetUserLibrary(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateArtist(int id, MusicArtist artist)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateUserLibrary(string username, UserMusicLibrary library)
        {
            throw new System.NotImplementedException();
        }
    }
}
