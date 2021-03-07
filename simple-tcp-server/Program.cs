using System;

namespace simple_tcp_server
{

    // This class is just a proof of concept!

    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                if (!Hosting.Server.IsRunning())
                {
                    Console.WriteLine("Type: [Host] or [Join]");
                    string value = Console.ReadLine().ToLower();
                    if (value == "host")
                    {
                        Hosting.Server.StartServer(true);
                    }
                    else if (value == "join")
                    {
                        Console.WriteLine("Enter IP:");
                        string ip = Console.ReadLine();
                        Connecting.Client.ConnectToServer(ip);
                    }
                }
                string input = Console.ReadLine().ToLower();

                if (input == "quit")
                    Connecting.Client.Disconnect();
            }
        }
    }
}
