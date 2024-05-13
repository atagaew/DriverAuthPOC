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
                LogRepository();
            }

            return tokenAdded;
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

        private void LogRepository()
        {
            Console.Clear();
            Console.WriteLine($"|                 Client                 |                     Token                      |");
            foreach (var item in _tokenStorage)
            {
                Console.WriteLine($"|  {item.Key}  |  {item.Value}  |");
            }
        }
    }
}
