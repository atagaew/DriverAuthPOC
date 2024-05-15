namespace POC.DriverInstanceLongPollingApp
{
    public class LongPollingDriverServiceClient
    {
        // todo use dependency injection to get httpclient from pool
        private static readonly HttpClient _client = new HttpClient() { BaseAddress = new Uri("http://localhost:5555/api/") };

        public async Task<string> GetCallbackUrlAsync(string clientId)
        {
            var urlResponse = await _client.GetAsync($"callbackurl?clientId={clientId}&type=lp");
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
                var tokenResponse = await _client.GetAsync($"token?clientId={clientId}");
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
                    await Task.Delay(1000);
                }
            }

            return string.Empty;
        }
    }
}
