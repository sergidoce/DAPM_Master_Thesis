using DAPM.PeerApi.Services.Interfaces;
using System.Text;

namespace DAPM.PeerApi.Services
{
    public class HttpService : IHttpService
    {
        private IHttpClientFactory _httpClientFactory;
        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendPostRequestAsync(string url, string body)
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
