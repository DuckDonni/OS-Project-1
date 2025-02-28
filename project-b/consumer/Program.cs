using System;
using System.IO;

class Consumer
{
    static void Main()
    {
        // Path to the named pipe
        string pipePath = "/tmp/my_pipe";

        // Checks if pipe exists
        if (!File.Exists(pipePath))
        {
            Console.WriteLine("Pipe does not exist.");
            return;
        }

        // If pipe exists the program opens the pipe for readings.
        using (var reader = new StreamReader(pipePath))
        {
            Console.WriteLine("Consumer has started. Waiting for messages...");

            // Makes the program read indefinitely
            while (true)
            {
                // When a message is available the program reads it and prints it to the console
                string? message = reader.ReadLine();
                if (message == null)
                {
                    // Creates delay before checking for new messages
                    System.Threading.Thread.Sleep(100);
                    continue;
                }

                Console.WriteLine($"Received: {message}");
            }
        }
    }
}
