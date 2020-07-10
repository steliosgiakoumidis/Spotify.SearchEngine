namespace Spotify.SearchEngine.Infrastructure.Models
{
    public class GetTokenReponse
    {
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }
        public string Scope { get; set; }
    }
}
