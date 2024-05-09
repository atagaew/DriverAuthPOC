using System.Text.Json;
using System.Text;

namespace POC.Common
{
    public class OAuthServiceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> LoginAsync(string? userName, string? password, string oauthurl)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, oauthurl);

            var requestData = new LoginRequest(userName, password, oauthurl);

            request.Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
