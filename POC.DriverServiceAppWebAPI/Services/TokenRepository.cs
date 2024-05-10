using System.Collections.Concurrent;

namespace POC.DriverServiceAppWebAPI.Services
{
    public class TokenRepository
    {
        private readonly ConcurrentDictionary<string, string> _tokenStorage = new();

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

            // todo use Results here
            return null;
        }
    }
}
