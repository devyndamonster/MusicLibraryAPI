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
            try
            {
                ItemResponse<MusicArtist> response = await _artistContainer.ReadItemAsync<MusicArtist>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch(CosmosException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<UserMusicLibrary> GetUserLibrary(string username)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateArtist(string id, MusicArtist artist)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateUserLibrary(string username, UserMusicLibrary library)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddArtist(MusicArtist artist)
        {
            Console.WriteLine($"Adding Artist: {artist.ArtistName}");
            await _artistContainer.CreateItemAsync<MusicArtist>(artist, new PartitionKey(artist.Id));
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
