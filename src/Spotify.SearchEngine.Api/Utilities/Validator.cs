using System;

namespace Spotify.SearchEngine.Api.Utilities
{
    public class Validator
    {
        public bool ValidateArtistsName(string artistName)
        {
            return !String.IsNullOrEmpty(artistName) && !String.IsNullOrWhiteSpace(artistName);
        }
    }
}
