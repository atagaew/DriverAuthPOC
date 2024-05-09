using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Text.Json;

namespace POC.DriverInstanceApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                await Console.Out.WriteLineAsync("Provide username and password");
                return;
            }

            var userName = args[0];
            var password = args[1];

            
            var client = new DriverServiceClient();

            // geting hub url
            var connectUrl = await client.GetConnectionUrl();
            await client.CreateConnection(connectUrl, userName, password, StartWorkWithToken, LoginToOAuthService);

            var myId = Guid.NewGuid().ToString();
            Console.WriteLine($"My unique id: {myId}");

            // Pause
            await Console.Out.WriteLineAsync("Hit Enter to start");
            Console.ReadLine();

            await client.GetCallbackUrl(myId);

            Console.ReadLine();
            await client.Disconnect();
        }

        static void StartWorkWithToken(string token)
        {
            Console.WriteLine($"Token received: [{token}]. Start working...");
        }

        static async Task LoginToOAuthService(string userName, string password, string callbackUrl)
        {
            await new OAuthServiceClient().LoginAsync(userName, password, callbackUrl);
        }
    }

    public class DriverServiceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private HubConnection _hubConnection;

        public async Task<string> GetConnectionUrl()
        {
            var response = await _httpClient.GetAsync("http://localhost:5555/api/driverservice/get-connect-url"); // ????
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                await Console.Out.WriteLineAsync("Error: " + response.ToString());
                return "";
            }
        }

        public async Task CreateConnection(string connectionUrl, string userName, string password, Action<string> startWorkWithToken, Func<string, string, string, Task> loginToOAuthService)
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(connectionUrl, opts =>
            {
                opts.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
            }).Build();

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

        public async Task Disconnect()
        {
            await _hubConnection.StopAsync();
        }
    }

    public class OAuthServiceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task LoginAsync(string userName, string password, string oauthurl)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, oauthurl);

            var requestData = new LoginRequest(userName, password, oauthurl);

            request.Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
        }
    }

    public record LoginRequest(string UserName, string Password, string CallbackUrl);
}
