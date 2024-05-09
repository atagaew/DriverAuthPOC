using Microsoft.AspNetCore.Mvc;
using POC.Common;
using POC.OAuthServiceWebAPI.Nowy_folder;

namespace POC.OAuthServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = _userRepository.Find(request.UserName, request.Password);
            if (user == null)
            {
                return BadRequest();
            }

            var uri = new Uri(request.CallbackUrl);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var returnUrl = queryParameters["returnUrl"];
            returnUrl += $"&token={user.Token}";

            _httpClient.GetAsync(returnUrl);

            return Ok();
        }
    }
}
