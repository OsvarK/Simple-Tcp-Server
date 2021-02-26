using System;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.StartServer();
            while (true)
            {
                int value = int.Parse(Console.ReadLine());
            }
        }
    }
}
