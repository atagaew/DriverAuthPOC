using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.DriverServiceAppWebAPI.Models.Handlers;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverServiceController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IMediator _mediator;

        public DriverServiceController(AppSettings appSettings, IMediator mediator)
        {
            _appSettings = appSettings;
            _mediator = mediator;
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
        public async Task PublishToken([FromQuery]PublishTokenQuery request)
        {
            if (_appSettings.SimulateDelay)
            {
                await Task.Delay(5000);
            }

            await _mediator.Send(request);
        }

        [HttpGet("get-callback-url")]
        public async Task<IActionResult> GetCallbackUrl([FromQuery]GetCallbackQuery request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-token")]
        public async Task<IActionResult> GetToken([FromQuery]GetTokenQuery request)
        {
            var token = await _mediator.Send(request);
            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }
    }
}
