using System;

namespace simple_tcp_server.Data
{
    /// <summary>
    /// A class to log stuffs on the server and clients, Easy to modify.
    /// </summary>
    class Logger
    {
        /// <summary>Logging something.</summary>
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
