using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using POC.DriverServiceAppWebAPI.Hubs;
using POC.DriverServiceAppWebAPI.Services;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverServiceController : ControllerBase
    {
        private readonly IHubContext<DriverServiceHub> _hubContext;
        private readonly AppSettings _appSettings;
        private readonly TokenRepository _tokenRepository;

        public DriverServiceController(IHubContext<DriverServiceHub> hubContext, AppSettings appSettings, TokenRepository tokenRepository)
        {
            _appSettings = appSettings;
            _tokenRepository = tokenRepository;
            _hubContext = hubContext;
        }

        [HttpGet("get-hub-connect-url")]
        public async Task<IActionResult> GetHubConnectUrl()
        {
            if (!_appSettings.UseSignalR)
            {
                return Forbid();
            }

            return Ok(_appSettings.HubUrl);
        }

        [HttpGet("publish-token")]
        public async Task PublishToken(string clientId, string token)
        {
            if (_appSettings.SimulateDelay)
            {
                await Task.Delay(5000);
            }

            _tokenRepository.AddToken(clientId, token);

            if (_appSettings.UseSignalR)
            {
                // todo move to service
                await _hubContext.Clients.All.SendAsync("Message", "Received Token. Sending to appropriate client");
                await _hubContext.Clients.Group(clientId).SendAsync("Token", token);
            }
        }

        [HttpGet("get-callback-url")]
        public async Task<string> GetCallbackUrl(string clientId)
        {
            return _appSettings.CallbackUrl.Replace("{replaceClientId}", clientId);
        }

        [HttpGet("get-token")]
        public async Task<IActionResult> GetToken(string clientId)
        {
            var token = _tokenRepository.GetToken(clientId);
            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }
    }
}
