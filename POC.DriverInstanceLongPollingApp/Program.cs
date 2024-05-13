using Microsoft.Extensions.Configuration;
using POC.Common;

namespace POC.DriverInstanceLongPollingApp
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var autologin = false;
            var username = string.Empty;
            var password = string.Empty;
            if (args.Length == 2)
            {
                autologin = true;
                username = args[0];
                password = args[1];
            }

            var driverServiceClient = new DriverServiceClient();

            var clientId = Guid.NewGuid().ToString();
            if (autologin)
            {
                Console.WriteLine($"Client {username} {clientId} started");
            }
            else
            {
                Console.Write($"Client {clientId}. Hit Enter to Start ");
                Console.ReadLine();
            }

            Console.Write("Getting callback url from Driver Service...");
            var callbackUrl = await driverServiceClient.GetCallbackUrlAsync(clientId);
            if (string.IsNullOrEmpty(callbackUrl))
            {
                Console.WriteLine($"failed! Try again later");
                return;
            }
            Console.WriteLine($"success!\nUrl: {callbackUrl}\n");

            await LoginToOAuthService(callbackUrl, autologin, username, password);

            // todo wait for token in a separate thread and allow user to cancel and retry using input 
            var token = await driverServiceClient.GetToken(clientId);
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Start working with token...");
            }
            else
            {
                Console.WriteLine("Getting token from server failed. Finishing...");
            }

            Console.Write("Hit Enter to exit ");
            Console.ReadLine();
        }

        private static async Task LoginToOAuthService(string callbackUrl, bool autologin = false, string name = "", string pass = "")
        {
            var loginUtl = new LoginUtility();
            (string userName, string password) = autologin ? (name, pass) : loginUtl.ShowLoginForm();

            var oAuthService = new OAuthServiceClient();
            var isLoggedIn = false;
            while (!isLoggedIn)
            {
                isLoggedIn = await oAuthService.LoginAsync(userName, password, callbackUrl);
                if (!isLoggedIn)
                {
                    (userName, password) = loginUtl.ShowLoginForm();
                }
            }
            Console.WriteLine("Successfully logged in!");
        }

    }
}
