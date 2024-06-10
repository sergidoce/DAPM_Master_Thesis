namespace DAPM.PeerApi.Services.Interfaces
{
    public interface IHttpService
    {
        public Task<string> SendPostRequestAsync(string url, string body);
    }
}
