using Microsoft.AspNetCore.SignalR.Client;

namespace POC.DriverInstanceApp
{
    public class DriverServiceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private HubConnection _hubConnection;

        public async Task<string> GetConnectionUrl()
        {
            var response = await _httpClient.GetAsync("http://localhost:5555/api/driverservice/get-hub-connect-url"); // ????
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task CreateConnection(string connectionUrl, string userName, string password, Action<string> startWorkWithToken, Func<string, string, string, Task> loginToOAuthService)
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(connectionUrl, opts =>
            {
                opts.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
            }).Build();

            //todo move it to separated classes handlers
            _hubConnection.On<string>("Message", (message) =>
            {
                Console.WriteLine($"Web API Message: {message}");
            });

            _hubConnection.On<string>("CallBackUrl", (callbackUrl) =>
            {
                loginToOAuthService(userName, password, callbackUrl);
            });

            _hubConnection.On<string>("Token", (token) =>
            {
                startWorkWithToken(token);
            });

            await _hubConnection.StartAsync();

        }

        public async Task GetCallbackUrl(string clientId)
        {
            await _hubConnection.InvokeAsync("GetCallBackUrl", clientId);
        }

        public async Task Disconnect(string clientId)
        {
            await _hubConnection.InvokeAsync("RemoveFromGroup", clientId);
            await _hubConnection.StopAsync();
        }
    }
}
