using System;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.ConnectToSerer("192.168.0.33");
            
            while (true)
            {
                Console.ReadLine();
                int value = int.Parse( Console.ReadLine());
            }
        }
    }
}
