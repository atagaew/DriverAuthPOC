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

            var token = GenerateToken(userInfo.UserName);

            var uri = new Uri(callbackUrl);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var returnUrl = queryParameters["returnUrl"];

            returnUrl += $"&token={token}";

            _httpClient.GetAsync(returnUrl);
        }

        private string GenerateToken(string userName) 
        {
            return $"{userName}-{Guid.NewGuid()}";
        }
    }
}
