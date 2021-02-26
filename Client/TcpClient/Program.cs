using System;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.ConnectToSerer("77.218.34.179");
            
            while (true)
            {
                Console.ReadLine();
                int value = int.Parse( Console.ReadLine());
            }
        }
    }
}
