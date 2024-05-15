using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace POC.DriverInstanceWebSocketApp
{
    public class WebSocketDriverServiceClient
    {

        // todo use dependency injection to get httpclient from pool
        private static readonly HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5555/api/callbackurl") };
        private static readonly ClientWebSocket _wsClient = new ClientWebSocket();

        public async Task<string> GetCallbackUrlAsync(string clientId)
        {
            var urlResponse = await _httpClient.GetAsync($"?clientId={clientId}&type=ws");
            if (!urlResponse.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            var callbackUrl = await urlResponse.Content.ReadAsStringAsync();
            return callbackUrl;
        }


        public async Task<bool> ConnectToServiceAsync(string clientId)
        {
            try
            {
                await _wsClient.ConnectAsync(new Uri($"ws://localhost:5555/ws/token?clientId={clientId}"), CancellationToken.None);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<string> GetToken()
        {
            var token = "FAILED";
            try
            {
                await Task.Run(async () =>
                {
                    var buffer = new byte[1024];
                    while (true)
                    {
                        var result = await _wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal close", CancellationToken.None);
                            break;
                        }
                        token = Encoding.UTF8.GetString(buffer);
                        await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal close", CancellationToken.None);
                        break;
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return token;
            }

            return token;
        }
    }
}
