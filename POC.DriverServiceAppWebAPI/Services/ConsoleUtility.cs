using System.Collections.Concurrent;

namespace POC.DriverServiceAppWebAPI.Services
{
    public static class ConsoleUtility
    {
        public static void DisplayRepository(ConcurrentDictionary<string, string> _tokenStorage)
        {
            Console.Clear();
            Console.WriteLine($"|                 Client                 |                     Token                       |");
            foreach (var item in _tokenStorage)
            {
                Console.Write($"|  {item.Key}  |  {item.Value}  ");
                for (int i = 0; i < 45 - item.Value.Length; i ++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("|");
            }
        }
    }
}
