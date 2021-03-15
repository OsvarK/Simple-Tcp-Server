using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using simple_tcp_server.Data;

namespace simple_tcp_server.Hosting
{
    /// <summary>
    /// A super simple TCP server.
    /// </summary>
    class Server
    {
        private static Socket socket;
        private static List<ServerSlot> serverSlots;
		private static Thread connectionThread;


        /// <summary> Starting the tcp server.</summary>
        /// <param name="createClientInstanceAfterServerStartup"> If true, after the creation 
        /// of the server we are creating a client instance. Meaning we can use the server as both a client and server,
        /// no need for two running applications.</param>
        /// <param name="port"> Defines on what port the server should run on.</param>
        /// <param name="maxConnections"> Defines how many connected clients this server can have.</param>
        public static void StartServer(bool createClientInstanceAfterServerStartup = false, int port = 26950, int maxConnections = 4) 
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
            connectionThread = new Thread(ConnectionThread);
            connectionThread.Start();

            if (createClientInstanceAfterServerStartup)
            {
                Logger.Log("[Server] Creating a client instance...");
                Connecting.Client.ConnectToServer();
            }
        }

        /// <summary> The thread for listening for incoming connections to the server.</summary>
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
                            // Connection sucessfully, send registration (give client its id)
							slot.Connect(connectingSocket);
							Logger.Log($"[Server] Sending registration to {connectingSocket.RemoteEndPoint}...");
							ServerSend.Registration(slot.id);
							foundSlot = true;
							break;
						}
					}
					if (!foundSlot)
					{
                        // Connection failed, server is full
                        Logger.Log($"[Server] Failed to connect [{connectingSocket.RemoteEndPoint}]: Server is full!");
						ServerSend.Error("Server is full", connectingSocket);
						connectingSocket.Close();
					}
				}
                catch (ThreadAbortException) { }
				catch (Exception) {
                    OnServerAction.OnServerException();
                }
			}
        }

        /// <summary> Check if socket is still connected.</summary>
        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        /// <summary> Receiving Thread listens to a connected server slot to recive data.</summary>
        private static void ReceivingThread(object serverSlot) 
        {
            ServerSlot sSlot = (ServerSlot)serverSlot;
            Socket socket = sSlot.GetSocket();

            byte[] buffer;
            int readBytes;

            while (!sSlot.IsEmpty())
            {
                try
                {
                    buffer = new byte[socket.SendBufferSize];
                    readBytes = socket.Receive(buffer);
                    if (readBytes > 0)
                    {
                        // Packet recived, send to reader
                        PacketHandler.ServerReadData(buffer);
                    }
                }
                catch (SocketException)
                {
                    sSlot.Disconnect();
                }
                catch (Exception e)
                {
                    OnServerAction.OnServerException();
                }
            }
        }

        /// <summary> Check if server is running.</summary>
        public static bool IsRunning()
        {
            return socket != null;
        }

        /// <summary> Closing the server.</summary>
        public static void CloseServer()
        {
            if (!IsRunning())
                return;

            Logger.Log("[Server] Disconnecting clients...");
            GetConnectedClients().ForEach(delegate (ServerSlot slot) { slot.Disconnect(); });

            Logger.Log("[Server] Socked closed, You are now disconnected!");
            try
            {
                socket.Close();
            }
            catch (Exception){ }
            socket = null;
            connectionThread.Abort();
            OnServerAction.OnServerClose();
        }

        /// <summary>Retrives all connected clients.</summary>
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

        /// <summary>A server slot to store a connection</summary>
        public class ServerSlot
        {
            public readonly int id;
            private Socket socket;
			private Thread receivingThread;
            public ServerSlot(int id)
            {
                this.id = id;
            }

            /// <summary>Connect a client to the this slot.</summary>
            /// <param name="socket">connecting socket</param>
            public void Connect(Socket socket)
            {
                Logger.Log($"[Server] {socket.RemoteEndPoint} has connected!");
                this.socket = socket;
                receivingThread = new Thread(ReceivingThread);
                receivingThread.Start(this);
            }

            /// <summary>Clear this slots of its connection.</summary>
            public void Disconnect()
            {
                if(socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket = null;
                    Logger.Log($"[Client {id}] has disconnected!");
                    OnServerAction.OnClientDisconnected(id);
                }
            }

            /// <summary>Checks if slot is empty.</summary>
            public bool IsEmpty() { return socket == null; }

            /// <summary>Returns the connected clients socket.</summary>
            public Socket GetSocket() { return socket; }
        }
    }
}
