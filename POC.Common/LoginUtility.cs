namespace POC.Common
{
    public class LoginUtility
    {
        public (string, string) ShowLoginForm()
        {
            Console.WriteLine("*** Login to OAuthService ***");
            Console.WriteLine("hint:| admin:12345 | manager:54321 | employee:11111 |\n");

            Console.Write("Username: ");
            var userName = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            return (userName, password);
        }
    }
}
