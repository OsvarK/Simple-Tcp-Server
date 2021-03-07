using simple_tcp_server.Data;

namespace simple_tcp_server.Connecting
{
    /// <summary>
    /// In this class all incoming methods are beings represented. 
    /// (the data from the <c>ServerSend</c> class is triggering a corresponding method in here)
    /// </summary>
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
