using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using POC.DriverServiceAppWebAPI.Models.Handlers;
using POC.DriverServiceAppWebAPI.Services;
using System.Net.WebSockets;
using System.Text;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ConfigurationSettings _appSettings;
        private readonly IMediator _mediator;
        private readonly WebSocketConnectionsService _connectionService;

        public TokenController(ConfigurationSettings appSettings, IMediator mediator, WebSocketConnectionsService connectionService)
        {
            _appSettings = appSettings;
            _mediator = mediator;
            _connectionService = connectionService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetToken([FromQuery] GetTokenQuery request)
        {
            var token = await _mediator.Send(request);
            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        [HttpGet("lp/publish")]
        public async Task PublishToken([FromQuery] PublishTokenQuery request)
        {
            await _mediator.Send(request);
        }

        [HttpGet("ws/publish")]
        public async Task PublishToken([FromQuery] string clientId, [FromQuery] string token)
        {
            await SenTokenToClientAsync(clientId, token);
        }


        private async Task SenTokenToClientAsync(string clientId, string token)
        {
            var client = _connectionService.GetConnection(clientId);

            var tokenBytes = Encoding.UTF8.GetBytes(token);
            await client.SendAsync(new ArraySegment<byte>(tokenBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal close", CancellationToken.None);
        }
    }
}
