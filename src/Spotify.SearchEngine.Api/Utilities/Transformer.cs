using Spotify.SearchEngine.Api.DtoModels;
using Spotify.SearchEngine.Application.Models;

namespace Spotify.SearchEngine.Api.Utilities
{
    public class Transformer
    {
        public ArtistTracksUrlResponseDto ArtistTracksResponseModelToDto(ArtistTracksUrlResponse artistTracksUrl)
        {
            return new ArtistTracksUrlResponseDto
            {
                ArtistName = artistTracksUrl.ArtistName,
                TracksUrl = artistTracksUrl.TracksUrl
            };
        }
    }
}
