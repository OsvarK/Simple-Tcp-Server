using simple_tcp_server.Data;

namespace simple_tcp_server.Connecting
{
    public class ClientHandle
    {
        public static void Registration(Packet packet)
        {
            Client.ReciveRegistration((int)packet.data[0]);
        }

        public static void ErrorFromServer(Packet packet)
        {
            Logger.Log($"[Server Error] {packet.data[0]}");
        }
    }
}
