using System;
using System.Collections.Generic;

namespace simple_tcp_server.Data
{

    /// <summary>
    /// This class is handeling the transaction between the <c>ClientSend</c> -> <c>ServerHandle</c>. 
    /// vice versa for <c>ServerSend</c> -> <c>ClientHandle</c>
    /// </summary>
    class PacketHandler
    {
        /// <summary>Each enum represents a method in the <c>ServerSend</c> class.</summary>
        public enum ServerPackets
        {
            Registration,
            Error
        }

        /// <summary>Each enum represents a method in the <c>ClientSend</c> class.</summary>
        public enum ClientPackets
        {
            ConfirmRegistration
        }

        /// <summary>Each item in the dictionary is calling/represents a method in <c>ClientHandle</c> class.</summary>
        private readonly static Dictionary<ServerPackets, MethodHandler> clientHandle = new Dictionary<ServerPackets, MethodHandler>()
        {
            {ServerPackets.Registration, Connecting.ClientHandle.Registration },
            {ServerPackets.Error, Connecting.ClientHandle.ErrorFromServer }
        };

        /// <summary>Each item in the dictionary is calling/represents a method in <c>ServerHandle</c> class.</summary>
        private readonly static Dictionary<ClientPackets, MethodHandler> serverHandle = new Dictionary<ClientPackets, MethodHandler>()
        {
            {ClientPackets.ConfirmRegistration, Hosting.ServerHandle.ConfirmRegistration }
        };

        public delegate void MethodHandler(Packet packet);

        /// <summary>Reades bytes and then decides what method in <c>ClientHandle</c> it should call.</summary>
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

        /// <summary>Reades bytes and then decides what method in <c>ServerHandle</c> it should call.</summary>
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