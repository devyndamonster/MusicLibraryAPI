using Bogus;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MusicLibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Services
{


    public class CosmosDbService : ICosmosDbService
    {
        private Container _userContainer;
        private Container _artistContainer;

        public CosmosDbService(CosmosClient client, string dbName, string userContainerName, string artistContainerName)
        {
            _userContainer = client.GetContainer(dbName, userContainerName);
            _artistContainer = client.GetContainer(dbName, artistContainerName);

            //Try and seed the database if it is empty
            //This is done in a "fire and forget" way
            Task.Run(async () => await SeedArtistContainerIfEmpty(_artistContainer, 20));
        }

        public async Task<MusicArtist> GetArtist(string id)
        {
            ItemResponse<MusicArtist> response = await _artistContainer.ReadItemAsync<MusicArtist>(id, new PartitionKey(id));
            return response.Resource;
        }


        /// <summary>
        /// Queries the Artist Container for music artists, sorted by their Id, and returns a list of size count artists starting at startIndex
        /// </summary>
        /// <param name="startIndex">The index of the first artist to be returned from the sorted database</param>
        /// <param name="count">Number of artists returned. Size of list may be less if count is larger than number of artists after startIndex</param>
        /// <returns>A list of artists</returns>
        public async Task<List<MusicArtist>> GetArtists(int startIndex, int count)
        {
            List<MusicArtist> artists = new List<MusicArtist>();

            //This is how you query cosmosDB asyncronously with LINQ according to the docs
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.container.getitemlinqqueryable?view=azure-dotnet

            //We orderby the Id because that is the pertition key, and ordering by name may be expensive
            using (FeedIterator<MusicArtist> setIterator = _artistContainer.GetItemLinqQueryable<MusicArtist>()
                .OrderBy(x => x.Id)
                .Skip(startIndex)
                .Take(count)
                .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (MusicArtist artist in await setIterator.ReadNextAsync())
                    {
                        artists.Add(artist);
                    }
                }
            }

            return artists;
        }

        public async Task UpdateArtist(string id, MusicArtist artist)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddArtist(MusicArtist artist)
        {
            await _artistContainer.CreateItemAsync(artist, new PartitionKey(artist.Id));
        }



        public async Task<UserMusicLibrary> CreateUserLibrary(string username)
        {
            UserMusicLibrary library = new UserMusicLibrary(username);

            ItemResponse<UserMusicLibrary> result = await _userContainer.CreateItemAsync(library, new PartitionKey(library.UserName));
            return result.Resource;
        }

        public async Task<UserMusicLibrary> GetUserLibrary(string username)
        {
            using (FeedIterator<UserMusicLibrary> setIterator = _userContainer.GetItemLinqQueryable<UserMusicLibrary>()
            .Where(o => o.UserName == username)
            .ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (UserMusicLibrary library in await setIterator.ReadNextAsync())
                    {
                        return library;
                    }
                }
            }

            return null;
        }


        public async Task<UserMusicLibrary> AddLikedArtist(string username, string artistId)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (!library.LikedArtistIds.Contains(artistId))
            {
                library.LikedArtistIds.Add(artistId);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }


        public async Task<UserMusicLibrary> RemoveLikedArtist(string username, string artistId)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (library.LikedArtistIds.Contains(artistId))
            {
                library.LikedArtistIds.Remove(artistId);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }



        public async Task<UserMusicLibrary> AddLikedAlbum(string username, MusicAlbum album)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (!library.LikedAlbums.Any(o => o.Id == album.Id))
            {
                library.LikedAlbums.Add(album);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }


        public async Task<UserMusicLibrary> RemoveLikedAlbum(string username, string albumId)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (library.LikedAlbums.Any(o => o.Id == albumId))
            {
                library.LikedAlbums.RemoveAll(o => o.Id == albumId);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }



        public async Task<UserMusicLibrary> AddLikedSong(string username, MusicSong song)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (!library.LikedAlbums.Any(o => o.Id == song.Id))
            {
                library.LikedSongs.Add(song);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }


        public async Task<UserMusicLibrary> RemoveLikedSong(string username, string songId)
        {
            UserMusicLibrary library = await GetUserLibrary(username);

            if (library.LikedSongs.Any(o => o.Id == songId))
            {
                library.LikedSongs.RemoveAll(o => o.Id == songId);
                ItemResponse<UserMusicLibrary> result = await _userContainer.UpsertItemAsync(library, new PartitionKey(library.UserName));
                return result.Resource;
            }

            else
            {
                return library;
            }
        }



        /// <summary>
        /// Generates a given number of music artists and adds them to the database
        /// </summary>
        /// <param name="artistContainer">The number of fake artists to generate</param>
        private async Task SeedArtistContainerIfEmpty(Container artistContainer, int numArtists)
        {

            //Get the number of artists in the DB asyncrounously
            //Based on an example found here: https://stackoverflow.com/questions/57516869/how-to-do-run-linq-count-on-a-cosmos-db-query-asynchronously-on-sdk-v3
            IOrderedQueryable<MusicArtist> linqQueryable = artistContainer.GetItemLinqQueryable<MusicArtist>();
            int artistCount = await linqQueryable.CountAsync();

            if (artistCount > 0) return;

            Console.WriteLine("Seeding Artist DB");

            string[] bandStrings = { "Horse", "Tablet", "Fire", "Wire", "Pencil", "Car", "TV", "Paper", "Headphones", "Water", "Bottle", "Metal", "Cup", "Mug", "Picture" };
            string[] albumStrings = { "Orange", "Blue", "Red", "Green", "Outside", "Inside", "Hard", "Soft", "Hot", "Cold", "Angry", "Sad", "Happy" };

            var randomArtist = new Faker<MusicArtist>()
                .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                .RuleFor(s => s.ArtistName, f => f.Name.FirstName() + " and the " + f.PickRandom(bandStrings))
                .FinishWith((artistFaker, artist) =>
                {
                    var randomAlbum = new Faker<MusicAlbum>()
                        .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                        .RuleFor(s => s.AlbumName, f => "The " + f.PickRandom(albumStrings) + " Album")
                        .RuleFor(s => s.ArtistId, f => artist.Id)
                        .FinishWith((albumFaker, album) => {

                            var randomSong = new Faker<MusicSong>()
                                .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                                .RuleFor(s => s.SongName, f => "A song for " + f.Name.FirstName())
                                .RuleFor(s => s.AlbumId, f => album.Id)
                                .RuleFor(s => s.AlbumName, f => album.AlbumName)
                                .RuleFor(s => s.ArtistId, f => artist.Id)
                                .RuleFor(s => s.ArtistName, f => artist.ArtistName);

                            album.Songs = randomSong.Generate(albumFaker.Random.Int(1, 10));

                        });

                    artist.Albums = randomAlbum.Generate(artistFaker.Random.Int(1, 5));
                });

            List<MusicArtist> artists = randomArtist.Generate(numArtists);

            //For each generated artist, add it to the cosmos db container
            foreach(MusicArtist artist in artists)
            {
                await AddArtist(artist);
            }

            Console.WriteLine("Seeding Complete!");
        }

        
    }
}
