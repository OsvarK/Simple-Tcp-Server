using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace simple_tcp_server.Data
{
    [Serializable]
    public class Packet
    {
        public List<object> data = new List<object>();
        public int packetId;
        public Packet(int packetId)
        {
            this.packetId = packetId;
        }
        public static byte[] Serialize(Packet packet)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, packet);
                return stream.ToArray();
            }
        }
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
