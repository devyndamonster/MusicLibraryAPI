﻿using MusicLibraryAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Services
{
    public interface ICosmosDbService
    {
        Task<UserMusicLibrary> GetUserLibrary(string username);
        Task<UserMusicLibrary> CreateUserLibrary(string username);
        Task<UserMusicLibrary> AddLikedArtist(string username, string artistId);
        Task<UserMusicLibrary> RemoveLikedArtist(string username, string artistId);
        Task<UserMusicLibrary> AddLikedAlbum(string username, MusicAlbum album);
        Task<UserMusicLibrary> RemoveLikedAlbum(string username, string albumId);
        Task<UserMusicLibrary> AddLikedSong(string username, MusicSong song);
        Task<UserMusicLibrary> RemoveLikedSong(string username, string songId);
        Task<MusicArtist> GetArtist(string id);
        Task<List<MusicArtist>> GetArtists(int startIndex, int count);
        Task UpdateArtist(string id, MusicArtist artist);
        Task AddArtist(MusicArtist artist);
    }
}
