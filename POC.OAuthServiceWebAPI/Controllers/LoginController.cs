using Microsoft.AspNetCore.Mvc;

namespace POC.OAuthServiceWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private static readonly List<UserInfo> _users = new List<UserInfo>
        {
            new UserInfo("admin", "12345", "AdminToken"),
            new UserInfo("manager", "54321", "ManagerToken"),
            new UserInfo("employee", "11111", "EmployeeToken"),
        };

        private static readonly HttpClient _httpClient = new HttpClient();

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = _users.FirstOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);
            if (user == null)
            {
                return BadRequest();
            }

            var uri = new Uri(request.CallbackUrl);
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var returnUrl = queryParameters["returnUrl"];
            returnUrl += $"&token={user.Token}";

            var resp = await _httpClient.GetAsync(returnUrl);

            return Ok();
        }
    }

    public record LoginRequest(string UserName, string Password, string CallbackUrl);

    public record UserInfo(string UserName, string Password, string Token);
}
