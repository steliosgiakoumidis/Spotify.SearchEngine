using System;

namespace Spotify.SearchEngine.Application
{
    public class ApplicationSettings
    {
        public string AuthenticationUrl { get; set; }
        public string SearchUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public ApplicationSettings()
        {
            AuthenticationUrl = Environment.GetEnvironmentVariable("AuthenticationUrl") ?? throw new ArgumentNullException();

            SearchUrl = Environment.GetEnvironmentVariable("SearchUrl") ??
                throw new ArgumentNullException();

            ClientId = Environment.GetEnvironmentVariable("ClientId") ??
                throw new ArgumentNullException();

            ClientSecret = Environment.GetEnvironmentVariable("ClientSecret") ??
                throw new ArgumentNullException();
        }
    }
}
