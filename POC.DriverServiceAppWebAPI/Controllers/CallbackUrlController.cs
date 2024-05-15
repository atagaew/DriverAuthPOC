using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.DriverServiceAppWebAPI.Models.Handlers;

namespace POC.DriverServiceAppWebAPI.Controllers
{
    // todo rename to be better rest API 
    [ApiController]
    [Route("api/[controller]")]
    public class CallbackUrlController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CallbackUrlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // todo rename to be better rest API 
        [HttpGet]
        public async Task<IActionResult> GetCallbackUrl([FromQuery]GetCallbackQuery request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
