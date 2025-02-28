using System;
using System.IO;
using System.Diagnostics;

class Producer
{
    static void Main()
    {
        string pipePath = "/tmp/my_pipe";

        // Checks if named pipe exists
        if (!File.Exists(pipePath))
        {
            // Creates new pipe if it does not exist
            Console.WriteLine("Creating named pipe...");
            Process.Start("mkfifo", pipePath)?.WaitForExit();
        }

        // Opens pipe for writing
        using (var writer = new StreamWriter(pipePath))
        {
            Console.WriteLine("Producer has started. Writing messages...");

            // Loop for writing messages to the pipe
            for (int i = 1; i <= 5; i++)
            {
                string message = $"Message {i} from Producer";
                writer.WriteLine(message);
                // Ensures data is written to pipe immediately
                writer.Flush(); 
                Console.WriteLine($"Sent: {message}");
                // Creates delay to show messages individually being sent
                System.Threading.Thread.Sleep(1000);
            }
        }

        Console.WriteLine("Producer finished writing.");
    }
}
