namespace POC.DriverInstanceLongPollingApp
{
    public class LongPollingDriverServiceClient
    {
        // todo use dependency injection to get httpclient from pool
        private static readonly HttpClient _client = new();
        private ConfigurationSettings _settings;

        public LongPollingDriverServiceClient(ConfigurationSettings settings)
        {
            _client.BaseAddress = new Uri(settings.DriverService.BaseUrl);
            _settings = settings;
        }

        public async Task<string> GetCallbackUrlAsync(string clientId)
        {
            var urlResponse = await _client.GetAsync(_settings.DriverService.CallbackUrlEndpoint.Replace("{{clientId}}", clientId));
            if (!urlResponse.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var callbackUrl = await urlResponse.Content.ReadAsStringAsync();
            return callbackUrl;
        }

        public async Task<string> GetToken(string clientId)
        {
            var isTokenReceived = false;
            var counter = 1;
            while (!isTokenReceived)
            {
                Console.Write($"{counter} Attempt to get token...");
                var tokenResponse = await _client.GetAsync(_settings.DriverService.TokenUrlEndpoint.Replace("{{clientId}}", clientId));
                isTokenReceived = tokenResponse.IsSuccessStatusCode;
                if (isTokenReceived)
                {
                    var token = await tokenResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"success!");
                    return token;
                }
                else
                {
                    Console.WriteLine($"no data.");
                    counter++;
                    await Task.Delay(_settings.LongPollingDelay);
                }
            }

            return string.Empty;
        }
    }
}
