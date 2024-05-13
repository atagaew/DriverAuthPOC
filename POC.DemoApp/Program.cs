using System.Diagnostics;

namespace POC.DemoApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started demo application");
            Console.Write("How many driver instances you want to run: ");
            int instances = int.Parse(Console.ReadLine());

            try
            {
                for (int i = 0; i < instances; i++)
                {
                    // Create a new process
                    Process process = new Process();

                    // Configure the process start information
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "POC.DriverInstanceLongPollingApp.exe",
                        UseShellExecute = true,
                        CreateNoWindow = false // Set this to true if you don't want a visible window for each instance
                    };

                    // Assign startInfo to the process
                    process.StartInfo = startInfo;

                    // Start the process
                    process.Start();

                    Console.WriteLine($"Instance {i + 1} started.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            Console.WriteLine("All instances started. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
