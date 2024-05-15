﻿using Microsoft.Extensions.Configuration;
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

            var driverServiceClient = new LongPollingDriverServiceClient();

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

            await OAuthManager.LoginToOAuthService(callbackUrl, autologin, username, password);

            // todo wait for token in a separate thread and allow user to cancel and retry using input 
            var token = await driverServiceClient.GetToken(clientId);
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
