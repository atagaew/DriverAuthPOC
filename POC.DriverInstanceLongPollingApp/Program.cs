using POC.Common;
using System;

namespace POC.DriverInstanceLongPollingApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5555/api/driverservice/");

            var clientId = Guid.NewGuid().ToString();
            Console.Write($"Client {clientId}. Hit Enter to Start ");
            Console.ReadLine();

            Console.Write("Getting callback url from Driver Service...");
            var urlResponse = await httpClient.GetAsync($"get-callback-url?clientId={clientId}");
            if (!urlResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"failed! Try again later");
                return;    
            }

            var callbackUrl = await urlResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"success! Url: {callbackUrl}\n");

            var loginUtl = new LoginUtility();
            (string userName, string password) = loginUtl.ShowLoginForm();

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


            var isTokenReceived = false;
            var counter = 1;
            while (!isTokenReceived)
            {
                Console.Write($"{counter} Attempt to get token...");
                var tokenResponse = await httpClient.GetAsync($"get-token?clientId={clientId}");
                isTokenReceived = tokenResponse.IsSuccessStatusCode;
                if (isTokenReceived)
                {
                    var token = await tokenResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"success! Received token from server: [{token}]");
                }
                else
                {
                    Console.WriteLine($"no data.");
                    counter++;
                    await Task.Delay(1000);
                }
            }

            Console.Write("Hit Enter to exit ");
            Console.ReadLine();
        }
    }
}
