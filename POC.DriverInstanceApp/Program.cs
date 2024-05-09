using POC.Common;

namespace POC.DriverInstanceApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var client = new DriverServiceClient();

            var clientId = Guid.NewGuid().ToString();
            Console.Write($"Client {clientId}. Hit Enter to Start ");
            Console.ReadLine();

            // geting hub url
            var connectUrl = await client.GetConnectionUrl();
            if (string.IsNullOrWhiteSpace(connectUrl))
            {
                Console.WriteLine("Cannot connect to server. Try later!");
                return;
            }

            var loginUtl = new LoginUtility();
            (string userName, string password) = loginUtl.ShowLoginForm();
            await client.CreateConnection(connectUrl, userName, password, StartWorkWithToken, LoginToOAuthService); 

            await client.GetCallbackUrl(clientId);
            
            Console.ReadLine();
            await client.Disconnect(clientId);
        }

        static void StartWorkWithToken(string token)
        {
            Console.WriteLine($"Token received: [{token}]. Start working...");
        }

        static async Task LoginToOAuthService(string userName, string password, string callbackUrl)
        {
            await new OAuthServiceClient().LoginAsync(userName, password, callbackUrl);
        }
    }
}
