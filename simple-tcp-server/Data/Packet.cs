using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace simple_tcp_server.Data
{

    /// <summary>
    /// This class represents a packet between the client and the server and vice versa.
    /// This class is beeing converted to bytes on transaction.
    /// </summary>
    [Serializable]
    public class Packet
    {
        public List<object> data = new List<object>();
        public int packetId;

        /// <summary>Creation of packet, int represents an enum from <c>PacketHandler</c>.</summary>
        public Packet(int packetId)
        {
            this.packetId = packetId;
        }

        /// <summary>Converts a packets class to bytes.</summary>
        public static byte[] Serialize(Packet packet)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, packet);
                return stream.ToArray();
            }
        }

        /// <summary>Converts bytes to a <c>Packet</c> class.</summary>
        public static Packet Deserialize(byte[] byteArray)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(byteArray))
            {
                return (Packet)formatter.Deserialize(stream);
            }
        }
    }
}
