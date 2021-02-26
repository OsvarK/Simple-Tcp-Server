using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace TcpClient
{
    class Client
    {
        private static Socket socket;
        public static void ConnectToSerer(string ip, int port = 26950)
        {
            try
            {
                socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
                LoopConnect(ip, port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void SendData(int value)
        {
            if (socket.Connected)
            {
                byte[] data = BitConverter.GetBytes(value);
                socket.Send(data);
            }
        }
        private static void RecieveThread()
        {
            Log($"Started listening to host...");

            byte[] buffer;
            int readBytes;

            while (socket.Connected)
            {
                try
                {
                    buffer = new byte[socket.SendBufferSize];
                    readBytes = socket.Receive(buffer);
                    if (readBytes > 0)
                    {
                        // TODO: read data
                    }
                }
                catch (SocketException)
                {
                    Disconnect();
                }
            }
        }
        public static void Disconnect()
        {
            //TODO: Send msg to server that i want to disconnect
            socket.Shutdown(SocketShutdown.Both);
            Log("Disconnected from the server");
        }
        private static void LoopConnect(string ip, int port)
        {
            int attempts = 0;
            while (!socket.Connected)
            {
                try
                {
                    attempts++;
                    Log("Trying to connect to server " + ip +":"+port.ToString());
                    socket.Connect(ip, port);
                }
                catch (SocketException)
                {
                    Log("Unable to connect to server, attempt: " + attempts.ToString());
                }
            }

            Log("You are now connected to the server!");
            Thread recieveThread = new Thread(RecieveThread);
            recieveThread.Start();
        }
    }
}
