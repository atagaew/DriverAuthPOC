using MediatR;
using Microsoft.AspNetCore.Mvc;
using POC.OAuthServiceWebAPI.Models.Handlers;
using POC.OAuthServiceWebAPI.Services;

namespace POC.OAuthServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly TokenService _tokenService;

        public AuthenticationController(IMediator mediator, TokenService tokenService)
        {
            _mediator = mediator;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var user = await _mediator.Send(request);
            if (user == null)
            {
                return BadRequest();
            }

            _tokenService.PublishUserToken(request.CallbackUrl, user);

            return Ok();
        }
    }
}
