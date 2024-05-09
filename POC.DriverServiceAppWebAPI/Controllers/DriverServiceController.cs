using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using POC.DriverServiceAppWebAPI.Hubs;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverServiceController : ControllerBase
    {
        private readonly IHubContext<DriverServiceHub> _hubContext;

        public DriverServiceController(IHubContext<DriverServiceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("get-connect-url")]
        public async Task<string> GetConnectUrl()
        {
            return "http://localhost:5555/driverServiceHub";
        }

        [HttpGet("publish-token")]
        public async Task PublishToken(string clientId, string token)
        {
            await _hubContext.Clients.All.SendAsync("Message", "Received Token. Sending to appropriate client");
            await _hubContext.Clients.Group(clientId).SendAsync("Token", token);
        }
    }
}
