using simple_tcp_server.Data;

namespace simple_tcp_server.Hosting
{
    class ServerHandle
    {
        public static void ConfirmRegistration(Packet packet)
        {
            Logger.Log($"[Server] Registration was confirmed by client[{packet.data[0]}]");
        }
    }
}
