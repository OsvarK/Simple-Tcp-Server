using System;
using System.Collections.Generic;
using System.Text;

namespace simple_tcp_server.Hosting
{
    class OnServerAction
    {
        public static void OnClientDisconnected(int clientId)
        {
            // Called on player disconnected
        }

        public static void OnClientConnected(int clientId)
        {
            // Called on player connected
        }
    }
}
