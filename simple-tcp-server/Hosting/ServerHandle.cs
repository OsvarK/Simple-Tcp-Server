using simple_tcp_server.Data;

namespace simple_tcp_server.Hosting
{
    /// <summary>
    /// In this class all incoming methods are beings represented. 
    /// (the data from the <c>ClientSend</c> class is triggering a corresponding method in here)
    /// </summary>
    class ServerHandle
    {
        public static void ConfirmRegistration(Packet packet)
        {
            Logger.Log($"[Server] Registration was confirmed by client[{packet.data[0]}]");
            OnServerAction.OnClientConnected((int)packet.data[0]);
        }
    }
}
