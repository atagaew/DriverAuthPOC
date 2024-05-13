using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace POC.DriverServiceAppWebAPI.Services
{
    public static class ConsoleUtility
    {
        public static void DisplayRepository(ConcurrentDictionary<string, string> _tokenStorage)
        {
            Console.WriteLine($"\n{DateTime.Now}\n");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"|                 Client                 |                     Token                       |");
            var immutableArray = _tokenStorage.ToImmutableArray();
            foreach (var item in immutableArray)
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
