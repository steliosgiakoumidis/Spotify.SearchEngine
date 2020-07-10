namespace Spotify.SearchEngine.Application.Models
{
    public class ActionResponse<T>
    {
        public bool IsSuccessful { get; private set; }
        public T ResponsePayload { get; set; }

        public ActionResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}
