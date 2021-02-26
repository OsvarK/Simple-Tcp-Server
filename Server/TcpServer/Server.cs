using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace TcpServer
{
    class Server
    {
        private static Socket listenerSocket;
        private static List<ServerSlot> slots;
        private static int maxClients = 4;
        private static bool serverRunning;

        public static void StartServer(int port = 26950)
        {
            try
            {
                InitServerVariables();
                listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Log($"Server started on port: {port}");
                serverRunning = true;
                listenerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                Thread listenerThread = new Thread(ListenerThread);
                listenerThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void ListenerThread()
        {
            while (serverRunning)
            {
                listenerSocket.Listen(0);
                Socket connectingSocket = listenerSocket.Accept();
                Log($"Incoming connection from [{connectingSocket.RemoteEndPoint}]...");
                bool foundSlot = false;
                foreach (ServerSlot slot in slots)
                {
                    if (slot.IsEmpty())
                    {
                        slot.ConnectSlot(connectingSocket);
                        foundSlot = true;
                        break;
                    }
                }
                if (!foundSlot)
                {
                    Log($"Failed to connect [{connectingSocket.RemoteEndPoint}]: Server is full!");
                    connectingSocket.Close();
                }
            }
        }
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        public static void InitServerVariables()
        {
            slots = new List<ServerSlot>();
            for (int i = 1; i <= maxClients; i++)
            {
                slots.Add(new ServerSlot(i));
            }
        }
    }

    class ServerSlot
    {
        public int id;
        private Socket socket;
        private Thread clientThread;

        public ServerSlot(int id)
        {
            this.id = id;
        }
        public void ConnectSlot(Socket socket)
        {
            this.socket = socket;
            Server.Log($"[Client {id}] [{socket.RemoteEndPoint}] has Connected!");
            clientThread = new Thread(RecieveThread);
            clientThread.Start(socket);
        }
        public void DisconnectSlot()
        {
            Server.Log($"[Client {id}] [{socket.RemoteEndPoint}] has disconnected!");
            socket.Shutdown(SocketShutdown.Both);
            socket = null;
            clientThread = null;
        }
        public Socket GetSocket() { return socket; }
        public bool IsEmpty() { return socket == null; }
        private void RecieveThread(object clientSocket)
        {
            Socket socket = (Socket)clientSocket;

            byte[] buffer;
            int readBytes;

            while (!IsEmpty())
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
                    DisconnectSlot();
                }
            }
        }
    }
}
