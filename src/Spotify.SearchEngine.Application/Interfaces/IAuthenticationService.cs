using System.Threading.Tasks;
using Spotify.SearchEngine.Application.Models;

namespace Spotify.SearchEngine.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ActionResponse<string>> GetAuthenticationToken();
    }
}
