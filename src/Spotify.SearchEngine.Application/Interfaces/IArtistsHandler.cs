using System.Collections.Generic;
using System.Threading.Tasks;
using Spotify.SearchEngine.Application.Models;

namespace Spotify.SearchEngine.Application.Interfaces
{
    public interface IArtistsHandler
    {
        Task<ActionResponse<List<ArtistTracksUrlResponse>>> GetArtistsTracks(string artistName);
    }
}
