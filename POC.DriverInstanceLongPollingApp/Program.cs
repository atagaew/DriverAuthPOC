using Microsoft.Extensions.Configuration;
using POC.Common;

namespace POC.DriverInstanceLongPollingApp
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var settings = new ConfigSettings();
            configuration.GetSection("ConfigSettings").Bind(settings);

            var driverServiceClient = new DriverServiceClient();

            var clientId = Guid.NewGuid().ToString();
            if (settings.UseAutoMode)
            {
                Console.WriteLine($"Client {clientId} started");
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

            await LoginToOAuthService(callbackUrl, settings);

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

        private static async Task LoginToOAuthService(string callbackUrl, ConfigSettings settings)
        {
            var loginUtl = new LoginUtility();
            (string userName, string password) = settings.UseAutoMode ? ("admin", "12345") : loginUtl.ShowLoginForm();

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
