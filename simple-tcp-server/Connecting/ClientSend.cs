using simple_tcp_server.Data;

namespace simple_tcp_server.Connecting
{
    /// <summary>This class is triggering methods in the <c>ServerHandle</c> class.</summary>
    class ClientSend
    {
        public static void Send(Packet packet)
        {
            Client.GetSocket().Send(Packet.Serialize(packet));
        }

        public static void ConfirmRegistration()
        {
            Packet packet = new Packet((int)PacketHandler.ClientPackets.ConfirmRegistration);
            packet.data.Add(Client.GetId());
            Send(packet);
        }
    }
}
