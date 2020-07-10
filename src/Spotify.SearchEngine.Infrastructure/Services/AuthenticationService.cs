using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Spotify.SearchEngine.Application;
using Spotify.SearchEngine.Application.Interfaces;
using Spotify.SearchEngine.Application.Models;
using Spotify.SearchEngine.Infrastructure.Models;
using Spotify.SearchEngine.Infrastructure.Utilities;

namespace Spotify.SearchEngine.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationSettings _settings;
        private readonly IMemoryCache _cache;
        private readonly HelperMethods _helperMethods;

        public AuthenticationService(IHttpClientFactory clientFactory, ApplicationSettings settings, IMemoryCache cache, HelperMethods helperMethods)
        {
            _clientFactory = clientFactory;
            _settings = settings;
            _cache = cache;
            _helperMethods = helperMethods;
        }

        public async Task<ActionResponse<string>> GetAuthenticationToken()
        {
            if (_cache.TryGetValue(Constants.CacheTokenKey, out string accessToken))
                return new ActionResponse<string>(true) { ResponsePayload = accessToken };

            try
            {
                using var client = _clientFactory.CreateClient(Constants.ClientName);

                var encodedCredentials = Convert.ToBase64String(
                        Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));
                client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", encodedCredentials);
                var requestMessage = CreateRequestMessage();

                var response = await client.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                    return new ActionResponse<string>(false);

                var responseAsString = await response.Content.ReadAsStringAsync();
                var tokenResponse = _helperMethods.DeserializedTokenResponse<GetTokenReponse>(responseAsString);

                var cacheLifetimeInSeconds = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.Expires_In - 1);
                _cache.Set(Constants.CacheTokenKey, tokenResponse.Access_Token, cacheLifetimeInSeconds);

                return new ActionResponse<string>(true) { ResponsePayload = tokenResponse.Access_Token };
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured while getting authentication token. Exception: {ex}");

                return new ActionResponse<string>(false);
            }
        }

        private HttpRequestMessage CreateRequestMessage()
        {
            return new HttpRequestMessage(HttpMethod.Post, _settings.AuthenticationUrl)
            {
                Content = new StringContent(Constants.GrantTypeAuthenticationFlow, Encoding.UTF8,
                    Constants.MediaTypeAuthenticationFlow)
            };
        }
    }
}
