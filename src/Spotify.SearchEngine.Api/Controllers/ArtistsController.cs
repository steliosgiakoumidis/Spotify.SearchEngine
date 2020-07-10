using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spotify.SearchEngine.Api.DtoModels;
using Spotify.SearchEngine.Api.Utilities;
using Spotify.SearchEngine.Application.Interfaces;

namespace Spotify.SearchEngine.Api.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly Validator _validator;
        private readonly Transformer _transformer;
        private readonly IArtistsHandler _handler;

        public ArtistsController(Validator validator, Transformer transformer, IArtistsHandler handler)
        {
            _validator = validator;
            _transformer = transformer;
            _handler = handler;
        }

        // GET: v1/name/{artistname}
        /// <summary>
        /// You pass the name of the artist and the response is a list of all matching artist names with a url with the tracks and albums of every match
        /// </summary>
        /// <returns>a list of urls and artist names</returns>
        ///<response code="400">Invalid incoming data</response>
        ///<response code="404">Artist not found</response>
        ///<response code="500">An error occured</response>
        ///<response code="200">A list of matching artists name and url for tracks and albums</response>
        [HttpGet]
        [Route("v1/name/{artistName}")]
        public async Task<ActionResult<List<ArtistTracksUrlResponseDto>>> GetArtists(string artistName)
        {
            if (!_validator.ValidateArtistsName(artistName))
                return BadRequest();

            var response = await _handler.GetArtistsTracks(artistName);
            if (!response.IsSuccessful)
                return StatusCode(500, "An error occured, please try again later");

            if (response.ResponsePayload.Count == 0)
                return NotFound("The artist was not found");

            return Ok(response.ResponsePayload.Select(x => _transformer.ArtistTracksResponseModelToDto(x)));
        }
    }
}