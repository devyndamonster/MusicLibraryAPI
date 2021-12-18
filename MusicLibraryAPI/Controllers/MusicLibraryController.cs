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
            return Ok("return some albums");
        }


        [HttpGet("user/albums")]
        public async Task<ActionResult> GetLikedAlbums([FromQuery] string username)
        {
            return Ok("return some albums");
        }


        [HttpPut("user/albums")]
        public async Task<ActionResult> AddLikedAlbum([FromQuery] string username, [FromQuery] int artistId, [FromQuery] int albumId)
        {
            return Ok("add some albums");
        }

        [HttpGet("user/songs")]
        public async Task<ActionResult> GetLikedSongs([FromQuery] string username)
        {
            return Ok("return some songs");
        }


        [HttpPut("user/songs")]
        public async Task<ActionResult> AddLikedSongs([FromQuery] string username, [FromQuery] int artistId, [FromQuery] int albumId, [FromQuery] int songId)
        {
            return Ok("add some songs");
        }

        [HttpGet("user/artists")]
        public async Task<ActionResult> GetLikedArtists([FromQuery] string username)
        {
            return Ok("return some artists");
        }


        [HttpPut("user/artists")]
        public async Task<ActionResult> AddLikedArtist([FromQuery] string username, [FromQuery] int artistId)
        {
            return Ok("add some artists");
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
