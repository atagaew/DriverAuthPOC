using Microsoft.AspNetCore.SignalR;

namespace POC.DriverServiceAppWebAPI.Hubs
{
    public class DriverServiceHub : Hub
    {
        private static string CALLBACK_URL = "http://localhost:7777/api/login/login?&returnUrl=http://localhost:5555/api/driverservice/publish-token?clientId={replaceClientId}";
        public async Task GetCallBackUrl(string clientId)
        {
            // register client
            await Groups.AddToGroupAsync(Context.ConnectionId, clientId);

            // form callback url
            var response = CALLBACK_URL.Replace("{replaceClientId}", clientId);

            // response to client
            await Clients.Group(clientId).SendAsync("Message", $"Client [{clientId}] added");
            await Clients.Group(clientId).SendAsync("CallBackUrl", response);
        }

        public async Task RemoveFromGroup(string clientId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, clientId);
            await Clients.Group(clientId).SendAsync("Message", $"Client [{clientId}] removed");
        }
    }
}
