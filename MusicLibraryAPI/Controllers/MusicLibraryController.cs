using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MusicLibraryController> _logger;

        public MusicLibraryController(ILogger<MusicLibraryController> logger)
        {
            _logger = logger;
        }


        [HttpGet("albums")]
        public ActionResult GetLikedAlbums([FromQuery] string username)
        {
            return Ok("return some albums");
        }


        [HttpPut("albums")]
        public ActionResult AddLikedAlbum([FromQuery] string username, [FromQuery] int albumId)
        {
            return Ok("add some albums");
        }

        [HttpGet("songs")]
        public ActionResult GetLikedSongs([FromQuery] string username)
        {
            return Ok("return some songs");
        }


        [HttpPut("songs")]
        public ActionResult AddLikedSongs([FromQuery] string username, [FromQuery] int songId)
        {
            return Ok("add some songs");
        }

        [HttpGet("artists")]
        public ActionResult GetLikedArtists([FromQuery] string username)
        {
            return Ok("return some artists");
        }


        [HttpPut("artists")]
        public ActionResult AddLikedArtist([FromQuery] string username, [FromQuery] int artistId)
        {
            return Ok("add some artists");
        }
    }
}
