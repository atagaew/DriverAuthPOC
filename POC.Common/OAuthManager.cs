namespace POC.Common
{
    public class OAuthManager
    {
        public static async Task LoginToOAuthService(string callbackUrl, bool autologin = false, string name = "", string pass = "")
        {
            var loginUtl = new LoginUtility();
            (string userName, string password) = autologin ? (name, pass) : loginUtl.ShowLoginForm();

            var oAuthService = new OAuthServiceClient();
            var isLoggedIn = false;
            while (!isLoggedIn)
            {
                Console.Write("Login to OAuthService...");
                isLoggedIn = await oAuthService.LoginAsync(userName, password, callbackUrl);
                if (!isLoggedIn)
                {
                    Console.WriteLine("fail!");
                    (userName, password) = loginUtl.ShowLoginForm();
                }
            }
            Console.WriteLine("success!");
        }
    }
}
