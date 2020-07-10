using System.Collections.Generic;
using System.Threading.Tasks;
using Spotify.SearchEngine.Application.Models;

namespace Spotify.SearchEngine.Application.Interfaces
{
    public interface ISearchService
    {
        Task<ActionResponse<List<ArtistTracksUrlResponse>>> GetTracksUrlForArtist(string artistName, string authenticationToken);
    }
}
