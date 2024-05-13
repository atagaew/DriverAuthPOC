using System.Diagnostics;

namespace POC.DemoApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started demo application");
            RunDriverInstances();
            Console.WriteLine("All instances started.\n");

            while (true)
            {
                Console.Write("Do you want to run more instances? y\\n");
                var userInput = Console.ReadLine();
                if ("n".Equals(userInput, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                RunDriverInstances();
            }

            Console.WriteLine("Hit Enter to exit");
            Console.ReadKey();
        }

        private static void RunDriverInstances()
        {
            var instances = new List<DriverInstances>();
            Console.WriteLine("Type, how many driver instances you want to run: ");
            
            int adminInstances = GetUserInputInstances("admin");
            instances.Add(new DriverInstances { Type = "admin 12345", Quantity = adminInstances });

            int managerInstances = GetUserInputInstances("manager"); ;
            instances.Add(new DriverInstances { Type = "manager 54321", Quantity = managerInstances });

            int employeeInstances = GetUserInputInstances("employee");
            instances.Add(new DriverInstances { Type = "employee 11111", Quantity = employeeInstances });

            try
            {
                foreach (var instance in instances)
                {
                    for (int i = 0; i < instance.Quantity; i++)
                    {
                        // Create a new process
                        Process process = new();

                        // Configure the process start information
                        ProcessStartInfo startInfo = new()
                        {
                            FileName = "POC.DriverInstanceLongPollingApp.exe",
                            Arguments = instance.Type,
                            UseShellExecute = true,
                            CreateNoWindow = false // Set this to true if you don't want a visible window for each instance
                        };

                        // Assign startInfo to the process
                        process.StartInfo = startInfo;

                        // Start the process
                        process.Start();

                        Console.WriteLine($"Instance {instance.Type.Split(" ")[0]} started.");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        private static int GetUserInputInstances(string type)
        {
            int result;
            bool isValidInput = false;

            do
            {
                Console.Write($"{type}: ");
                string input = Console.ReadLine();

                isValidInput = int.TryParse(input, out result);

                if (!isValidInput)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            } while (!isValidInput);

            return result;
        }
    }
}
