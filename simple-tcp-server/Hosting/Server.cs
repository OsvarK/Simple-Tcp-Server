using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using simple_tcp_server.Data;

namespace simple_tcp_server.Hosting
{
    class Server
    {
        private static Socket socket;
        private static List<ServerSlot> serverSlots;

        public static void StartServer(int port = 26950, int maxConnections = 1) 
        {
            if (IsRunning())
                return;

            Logger.Log("[Server] Initializing client slots");
            serverSlots = new List<ServerSlot>();
            for (int i = 1; i <= maxConnections; i++)
                serverSlots.Add(new ServerSlot(i));

            Logger.Log("[Server] Initializing socket");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));

            Logger.Log("[Server] Starts listening for connections");
            Thread connectionThread = new Thread(ConnectionThread);
            connectionThread.Start();

            Logger.Log("[Server] Creating a client instance...");
            Connecting.Client.ConnectToServer();
        }
        private static void ConnectionThread() 
        {
            while (IsRunning())
            {	
				try
				{
					socket.Listen(0);
					Socket connectingSocket = socket.Accept();
					Logger.Log($"[Server] Incoming connection from {connectingSocket.RemoteEndPoint}...");
					bool foundSlot = false;
					foreach (ServerSlot slot in serverSlots)
					{
						if (slot.IsEmpty())
						{
							slot.Connect(connectingSocket);
							Logger.Log($"[Server] Sending registration to {connectingSocket.RemoteEndPoint}...");
							ServerSend.Registration(slot.id);
							foundSlot = true;
							break;
						}
					}
					if (!foundSlot)
					{
						Logger.Log($"[Server] Failed to connect [{connectingSocket.RemoteEndPoint}]: Server is full!");
						ServerSend.Error("Server is full", connectingSocket);
						connectingSocket.Close();
					}
				}
				catch {}
			}
        }
        private static void ReceivingThread(object serverSlot) 
        {
            ServerSlot sSlot = (ServerSlot)serverSlot;
            Socket socket = sSlot.GetSocket();

            byte[] buffer;
            int readBytes;

            while (socket != null)
            {
                try
                {
                    buffer = new byte[socket.SendBufferSize];
                    readBytes = socket.Receive(buffer);
                    if (readBytes > 0)
                    {
                        PacketHandler.ServerReadData(buffer);
                    }
                }
                catch (SocketException)
                {
                    sSlot.Disconnect();
                }
            }
        }
        public static bool IsRunning()
        {
            return socket != null;
        }
		private static void CloseServerIfNoPlayers()
		{
		if (GetConnectedClients().Count == 0)
			Disconnect();
		}
        public static void Disconnect()
        {
            Logger.Log("[Server] Socked closed, You are now disconnected!");
            socket.Close();
            socket = null;
        }
        public static List<ServerSlot> GetConnectedClients() 
        {
            List<ServerSlot> clients = new List<ServerSlot>();
            foreach (ServerSlot slot in serverSlots)
            {
                if (!slot.IsEmpty())
                    clients.Add(slot);
            }
            return clients;
        }
        public class ServerSlot
        {
            public readonly int id;
            private Socket socket;
            public ServerSlot(int id)
            {
                this.id = id;
            }
            public void Connect(Socket socket)
            {
                Logger.Log($"[Server] {socket.RemoteEndPoint} has connected!");
                this.socket = socket;
                Thread receivingThread = new Thread(ReceivingThread);
                receivingThread.Start(this);
            }
            public void Disconnect()
            {
                if(socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket = null;
                    Logger.Log($"[Client {id}] has disconnected!");
					CloseServerIfNoPlayers();
                }
            }
            public bool IsEmpty() { return socket == null; }
            public Socket GetSocket() { return socket; }
        }
    }
}
