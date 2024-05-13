using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.DriverServiceAppWebAPI.Models.Handlers;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    // todo rename to be better rest API 
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

        [HttpGet("publish-token")]
        public async Task PublishToken([FromQuery]PublishTokenQuery request)
        {
            if (_appSettings.SimulateDelay)
            {
                await Task.Delay(new Random().Next(1, 15) * 1000);
            }

            await _mediator.Send(request);
        }

        // todo rename to be better rest API 
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
