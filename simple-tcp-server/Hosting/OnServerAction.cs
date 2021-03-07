using System;
using System.Collections.Generic;
using System.Text;

namespace simple_tcp_server.Hosting
{
    /// <summary>
    /// A class to call certain action when something is happening on the server (used for external use)
    /// </summary>
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
