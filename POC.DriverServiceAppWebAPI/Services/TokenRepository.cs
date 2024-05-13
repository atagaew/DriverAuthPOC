using System.Collections.Concurrent;

namespace POC.DriverServiceAppWebAPI.Services
{
    public class TokenRepository
    {
        private readonly ConcurrentDictionary<string, string> _tokenStorage = new();
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(ILogger<TokenRepository> logger)
        {
            _logger = logger;
        }

        public bool AddToken(string clientId, string token)
        {
            
            var tokenAdded = _tokenStorage.TryAdd(clientId, token);

            if (tokenAdded)
            {
                ConsoleUtility.DisplayRepository(_tokenStorage);
            }

            return tokenAdded;
        }

        public string GetToken(string clientId)
        {
            if (_tokenStorage.TryRemove(clientId, out var token))
            {
                return token;
            }

            // todo use Results here
            return null;
        }
    }
}
