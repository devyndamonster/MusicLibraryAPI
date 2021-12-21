using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibraryAPI.Models;
using MusicLibraryAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibraryAPI.Controllers
{
    [ApiController]
    [Route("musicapi")]
    public class MusicLibraryController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public MusicLibraryController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }


        [HttpGet("user/login")]
        public async Task<ActionResult> GetUserLogin([FromQuery] string username)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.GetUserLibrary(username);

                if(library == null)
                {
                    library = await _cosmosDbService.CreateUserLibrary(username);
                }

                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpGet("user/albums")]
        public async Task<ActionResult> GetLikedAlbums([FromQuery] string username)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.GetUserLibrary(username);
                return Ok(library.LikedAlbums);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpPut("user/albums")]
        public async Task<ActionResult> AddLikedAlbum([FromQuery] string username, [FromQuery] string artistId, [FromQuery] string albumId)
        {
            try
            {
                MusicArtist artist = await _cosmosDbService.GetArtist(artistId);
                MusicAlbum album = artist.Albums.FirstOrDefault(o => o.Id == albumId);

                UserMusicLibrary library = await _cosmosDbService.AddLikedAlbum(username, album);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }



        [HttpDelete("user/albums")]
        public async Task<ActionResult> RemoveLikedAlbum([FromQuery] string username, [FromQuery] string albumId)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.RemoveLikedAlbum(username, albumId);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }



        [HttpGet("user/songs")]
        public async Task<ActionResult> GetLikedSongs([FromQuery] string username)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.GetUserLibrary(username);
                return Ok(library.LikedSongs);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpPut("user/songs")]
        public async Task<ActionResult> AddLikedSongs([FromQuery] string username, [FromQuery] string artistId, [FromQuery] string albumId, [FromQuery] string songId)
        {
            try
            {
                MusicArtist artist = await _cosmosDbService.GetArtist(artistId);
                MusicAlbum album = artist.Albums.FirstOrDefault(o => o.Id == albumId);
                MusicSong song = album.Songs.FirstOrDefault(o => o.Id == songId);

                UserMusicLibrary library = await _cosmosDbService.AddLikedSong(username, song);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpDelete("user/songs")]
        public async Task<ActionResult> RemoveLikedSongs([FromQuery] string username, [FromQuery] string songId)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.RemoveLikedSong(username, songId);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }



        [HttpGet("user/artists")]
        public async Task<ActionResult> GetLikedArtists([FromQuery] string username)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.GetUserLibrary(username);

                List<MusicArtist> artists = new List<MusicArtist>();
                foreach(string id in library.LikedArtistIds)
                {
                    MusicArtist artist = await _cosmosDbService.GetArtist(id);
                    artists.Add(artist);
                }

                return Ok(artists);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpPut("user/artists")]
        public async Task<ActionResult> AddLikedArtist([FromQuery] string username, [FromQuery] string artistId)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.AddLikedArtist(username, artistId);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpDelete("user/artists")]
        public async Task<ActionResult> RemoveLikedArtist([FromQuery] string username, [FromQuery] string artistId)
        {
            try
            {
                UserMusicLibrary library = await _cosmosDbService.RemoveLikedArtist(username, artistId);
                return Ok(library);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }


        [HttpGet("search/artists")]
        public async Task<ActionResult> GetArtists([FromQuery] int startIndex, [FromQuery] int count)
        {
            try
            {
                List<MusicArtist> artists = await _cosmosDbService.GetArtists(startIndex, count);
                return Ok(artists);
            }
            catch(Exception e)
            {
                return Problem("Something bad happened: \n" + e.ToString());
            }
        }
    }
}
