using POC.Common;

namespace POC.OAuthServiceWebAPI.Services
{
    public class TokenService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task PublishUserToken(string callbackUrl, UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(callbackUrl) || userInfo == null)
            {
                return;
            }

            var uri = new Uri(callbackUrl);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var returnUrl = queryParameters["returnUrl"];

            var token = GenerateToken(returnUrl, userInfo.UserName);

            returnUrl += $"&token={token}";

            _httpClient.GetAsync(returnUrl);
        }

        private string GenerateToken(string returnUrl, string userName) 
        {
            var uri = new Uri(returnUrl);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var clientId = queryParameters["clientId"];

            return $"{clientId}-{userName}-{Guid.NewGuid()}";
        }
    }
}
