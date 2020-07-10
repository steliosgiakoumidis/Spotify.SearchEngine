using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using Spotify.SearchEngine.Application;
using Spotify.SearchEngine.Application.Interfaces;
using Spotify.SearchEngine.Application.Models;
using Spotify.SearchEngine.Infrastructure.Models.SearchResponse;
using Spotify.SearchEngine.Infrastructure.Utilities;

namespace Spotify.SearchEngine.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationSettings _settings;
        private readonly HelperMethods _helperMethods;

        public SearchService(IHttpClientFactory clientFactory, ApplicationSettings settings, HelperMethods helperMethods)
        {
            _clientFactory = clientFactory;
            _settings = settings;
            _helperMethods = helperMethods;
        }

        public async Task<ActionResponse<List<ArtistTracksUrlResponse>>> GetTracksUrlForArtist(string artistName, string authenticationToken)
        {

            try
            {
                using var client = _clientFactory.CreateClient(Constants.ClientName);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken);

                var response = await client.GetAsync(QueryHelpers.AddQueryString(_settings.SearchUrl, GetQueryParameters(artistName)));
                if (!response.IsSuccessStatusCode)
                    return new ActionResponse<List<ArtistTracksUrlResponse>>(false);

                var responseAsString = await response.Content.ReadAsStringAsync();
                var searchResponse = _helperMethods.DeserializedTokenResponse<SearchResponse>(responseAsString);

                return new ActionResponse<List<ArtistTracksUrlResponse>>(true)
                {
                    ResponsePayload = searchResponse.Artists.Items
                        .Select(x => new ArtistTracksUrlResponse()
                        {
                            ArtistName = x.Name,
                            TracksUrl = x.External_Urls.Spotify
                        })
                        .ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while searching artists. Exception: {ex}");

                return new ActionResponse<List<ArtistTracksUrlResponse>>(false);
            }
        }

        private Dictionary<string, string> GetQueryParameters(string artistName)
        {
            return new Dictionary<string, string>()
                {
                    {"q", artistName},
                    {"type", "artist" }
                };
        }
    }
}
