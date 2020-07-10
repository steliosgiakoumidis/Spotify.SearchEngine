using System.Text.Json;

namespace Spotify.SearchEngine.Infrastructure.Utilities
{
    public class HelperMethods
    {
        public T DeserializedTokenResponse<T>(string responseAsString)
        {
            return JsonSerializer.Deserialize<T>(responseAsString,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = true
                    });
        }
    }
}
