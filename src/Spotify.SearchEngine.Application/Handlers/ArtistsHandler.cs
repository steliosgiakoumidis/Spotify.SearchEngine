using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotify.SearchEngine.Application.Interfaces;
using Spotify.SearchEngine.Application.Models;

namespace Spotify.SearchEngine.Application.Handlers
{
    public class ArtistsHandler : IArtistsHandler
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISearchService _searchService;

        public ArtistsHandler(IAuthenticationService authenticationService, ISearchService searchService)
        {
            _authenticationService = authenticationService;
            _searchService = searchService;
        }

        public async Task<ActionResponse<List<ArtistTracksUrlResponse>>> GetArtistsTracks(string artistName)
        {
            var token = await _authenticationService.GetAuthenticationToken();
            if (!token.IsSuccessful || string.IsNullOrEmpty(token.ResponsePayload))
                return new ActionResponse<List<ArtistTracksUrlResponse>>(false);

            var artistTrackUrls = await _searchService.GetTracksUrlForArtist(artistName, token.ResponsePayload);
            if (!artistTrackUrls.IsSuccessful || artistTrackUrls == null)
                return new ActionResponse<List<ArtistTracksUrlResponse>>(false);

            var filteredArtistTrackUrls = FilterArtistsResults(artistName, artistTrackUrls.ResponsePayload);

            return new ActionResponse<List<ArtistTracksUrlResponse>>(true) { ResponsePayload = filteredArtistTrackUrls };
        }

        private List<ArtistTracksUrlResponse> FilterArtistsResults(string artistName, List<ArtistTracksUrlResponse> artistTrackUrls)
        {
            return artistTrackUrls.Where(x => NormalizeArtistName(x.ArtistName) == NormalizeArtistName(artistName)).ToList();
        }

        private string NormalizeArtistName(string artistName)
        {
            return new string(artistName.Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c)).ToArray()).ToLower();
        }
    }
}
