using System.Net.Sockets;
using simple_tcp_server.Data;

namespace simple_tcp_server.Hosting
{
    class ServerSend
    {
        #region send types
        public static void Send(Packet packet, int toId)
        {
            byte[] bytes = Packet.Serialize(packet);
            foreach (Server.ServerSlot slot in Server.GetConnectedClients())
            {
                if (slot.id == toId)
                    slot.GetSocket().Send(bytes);
            }
        }
        public static void Send(Packet packet, Socket socket)
        {
            byte[] bytes = Packet.Serialize(packet);
            socket.Send(bytes);
        }
        public static void SendToAll(Packet packet)
        {
            byte[] bytes = Packet.Serialize(packet);
            foreach (Server.ServerSlot slot in Server.GetConnectedClients())
                slot.GetSocket().Send(bytes);
        }
        #endregion

        public static void Registration(int id)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.Registration);
            packet.data.Add(id);
            Send(packet, id);
        }

        public static void Error(string msg, int toId)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.Error);
            packet.data.Add(msg);
            Send(packet, toId);
        }

        public static void Error(string msg, Socket socket)
        {
            Packet packet = new Packet((int)PacketHandler.ServerPackets.Error);
            packet.data.Add(msg);
            Send(packet, socket);
        }
    }
}
