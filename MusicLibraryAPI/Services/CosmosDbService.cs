using Bogus;
using Microsoft.Azure.Cosmos;
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

            SeedArtistContainerIfEmpty(_artistContainer, 20);
        }

        public async Task<MusicArtist> GetArtist(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserMusicLibrary> GetUserLibrary(string username)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateArtist(int id, MusicArtist artist)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateUserLibrary(string username, UserMusicLibrary library)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddArtist(MusicArtist artist)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Generates a given number of music artists and adds them to the database
        /// </summary>
        /// <param name="artistContainer">The number of fake artists to generate</param>
        private void SeedArtistContainerIfEmpty(Container artistContainer, int numArtists)
        {
            int artistCount = artistContainer.GetItemLinqQueryable<MusicArtist>().Count();
            if (artistCount > 0) return;

            string[] bandStrings = { "Horse", "Tablet", "Fire", "Wire", "Pencil", "Car", "TV", "Paper", "Headphones", "Water", "Bottle", "Metal", "Cup", "Mug", "Picture" };
            string[] albumStrings = { "Orange", "Blue", "Red", "Green", "Outside", "Inside", "Hard", "Soft", "Hot", "Cold", "Angry", "Sad", "Happy" };

            var randomArtist = new Faker<MusicArtist>()
                .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                .RuleFor(s => s.ArtistName, f => f.Name + " and the " + f.PickRandom(bandStrings))
                .FinishWith((artistFaker, artist) =>
                {
                    var randomAlbum = new Faker<MusicAlbum>()
                        .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                        .RuleFor(s => s.AlbumName, f => "The " + f.PickRandom(albumStrings) + " Album")
                        .RuleFor(s => s.ArtistId, f => artist.Id)
                        .FinishWith((albumFaker, album) => {

                            var randomSong = new Faker<MusicSong>()
                                .RuleFor(s => s.Id, f => Guid.NewGuid().ToString())
                                .RuleFor(s => s.SongName, f => "A song for " + f.Name)
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
            //This calls async tasks in a blocking fashion, but that's okay since this only runs once on an empty db
            foreach(MusicArtist artist in artists)
            {
                Task.Run(() => AddArtist(artist)).Wait();
            }
        }
    }
}
