using System;
using System.Collections.Generic;
using System.Text;

using simple_tcp_server.Hosting;

namespace simple_tcp_server.Connecting
{
    class OnClientAction
    {
        public static void OnClientDisconnect()
        {
            if (Server.IsRunning())
                Server.CloseServer();
        }

        public static void OnClientException()
        {
            if (!Client.IsRunning())
                return;
            Client.Disconnect();
        }

        public static void UnableToConnectToServer()
        {
            OnClientException();
        }

        public static void OnClientSuccessfullyConnected()
        {
            ClientSend.ConfirmRegistration();
        }

        public static void LostConnectionToServer()
        {
            OnClientException();
        }
    }
}
