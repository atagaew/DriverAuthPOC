namespace POC.Common
{
    public record LoginRequest(string UserName, string Password, string CallbackUrl);

    public record UserInfo(string UserName, string Password);
}
