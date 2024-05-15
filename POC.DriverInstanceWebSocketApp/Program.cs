using Microsoft.Extensions.Configuration;
using POC.Common;

namespace POC.DriverInstanceWebSocketApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("wsAppsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var settings = new ConfigurationSettings();
            configuration.GetSection(ConfigurationSettings.SectionName).Bind(settings);

            var autologin = false;
            var username = string.Empty;
            var password = string.Empty;
            if (args.Length == 2)
            {
                autologin = true;
                username = args[0];
                password = args[1];
            }

            var driverServiceClient = new WebSocketDriverServiceClient(settings);

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
            Console.WriteLine($"success!\nCallbackUrl: {callbackUrl}\n");

            Console.Write("Connecting to server to receive token...");
            if (!await driverServiceClient.ConnectToServiceAsync(clientId))
            {
                Console.WriteLine($"failed! Try again later");
                return;
            }
            Console.WriteLine("connected!");

            var loginUrl = $"{settings.OAuthUrl}?returnUrl={callbackUrl}";
            await OAuthManager.LoginToOAuthService(loginUrl, autologin, username, password);

            // todo wait for token in a separate thread and allow user to cancel and retry using input 
            var token = await driverServiceClient.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"Received token: [{token}]");
            }
            else
            {
                Console.WriteLine("Getting token from server failed. Finishing...");
            }

            Console.Write("Hit Enter to exit ");
            Console.ReadLine();
        }
    }
}
