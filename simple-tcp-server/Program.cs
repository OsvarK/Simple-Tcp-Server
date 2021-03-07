using System;

namespace simple_tcp_server
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                if (!Hosting.Server.IsRunning())
                {
                    Console.WriteLine("Type: [Host] or [Join]");
                    string value = Console.ReadLine();
                    if (value == "Host")
                    {
                        Hosting.Server.StartServer(true);
                    }
                    else if (value == "Join")
                    {
                        Console.WriteLine("Enter IP:");
                        string ip = Console.ReadLine();
                        Connecting.Client.ConnectToServer(ip);
                    }
                }
                string input = Console.ReadLine();

                if (input == "quit")
                    Connecting.Client.Disconnect();
            }
        }
    }
}
