using System.Collections.Concurrent;

namespace POC.DriverServiceAppWebAPI.Services
{
    public class TokenRepository
    {
        private ConcurrentDictionary<string, string> _tokenStorage = new ConcurrentDictionary<string, string>();

        public bool AddToken(string clientId, string token)
        {
            return _tokenStorage.TryAdd(clientId, token);
        }

        public string GetToken(string clientId)
        {
            if (_tokenStorage.TryGetValue(clientId, out var token))
            {
                return token;
            }

            return null;
        }
    }
}
