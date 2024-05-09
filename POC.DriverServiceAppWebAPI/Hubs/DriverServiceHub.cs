using Microsoft.AspNetCore.SignalR;

namespace POC.DriverServiceAppWebAPI.Hubs
{
    public class DriverServiceHub : Hub
    {
        private readonly AppSettings _appSettings;

        public DriverServiceHub(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task GetCallBackUrl(string clientId)
        {
            // register client
            await Groups.AddToGroupAsync(Context.ConnectionId, clientId);

            // todo move to separate class method
            // form callback url
            var response = _appSettings.CallbackUrl.Replace("{replaceClientId}", clientId);

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
