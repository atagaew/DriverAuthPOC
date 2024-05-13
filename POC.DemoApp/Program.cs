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
            Console.Write("admin: ");
            int adminInstances = int.Parse(Console.ReadLine());
            instances.Add(new DriverInstances { Type = "admin 12345", Quantity = adminInstances });
            Console.Write("manager: ");
            int managerInstances = int.Parse(Console.ReadLine());
            instances.Add(new DriverInstances { Type = "manager 54321", Quantity = managerInstances });
            Console.Write("employee: ");
            int employeeInstances = int.Parse(Console.ReadLine());
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

                        Console.WriteLine($"Instance {instance.Type} started.");
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
    }

    class DriverInstances
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
    }
}
