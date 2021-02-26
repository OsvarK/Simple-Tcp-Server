using System;
using System.Collections.Generic;

namespace simple_tcp_server.Data
{
    class PacketHandler
    {

        // ServerSend -> ClientHandle
        // ClientSend -> ServerHandle


        // From server to client
        public enum ServerPackets
        {
            Registration,
            Error
        }

        // From client to server
        public enum ClientPackets
        {
            ConfirmRegistration
        }
    
        // Define packets from server
        private readonly static Dictionary<ServerPackets, MethodHandler> clientHandle = new Dictionary<ServerPackets, MethodHandler>()
        {
            {ServerPackets.Registration, Connecting.ClientHandle.Registration },
            {ServerPackets.Error, Connecting.ClientHandle.ErrorFromServer }
        };

        // Define packets from client
        private readonly static Dictionary<ClientPackets, MethodHandler> serverHandle = new Dictionary<ClientPackets, MethodHandler>()
        {
            {ClientPackets.ConfirmRegistration, Hosting.ServerHandle.ConfirmRegistration }
        };

        public delegate void MethodHandler(Packet packet);
        public static void ClientReadData(byte[] bytes)
        {
            try
            {
                Packet packet = Packet.Deserialize(bytes);
                MethodHandler method = clientHandle[(ServerPackets) packet.packetId];
                method(packet);
            }
            catch (Exception)
            {

            }
        }
        public static void ServerReadData(byte[] bytes)
        {
            try
            {
                Packet packet = Packet.Deserialize(bytes);
                MethodHandler method = serverHandle[(ClientPackets)packet.packetId];
                method(packet);
            }
            catch (Exception)
            {

            }
        }
    }
}