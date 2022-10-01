using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.TCP
{
    public class Client : IDisposable
    {
        private TcpClient _client;
        private int myPort;
        private string myHost;
        public Client(string host, int port)
        {
            myHost = host;
            myPort = port;
            _client = new TcpClient();
            _client.Connect(myHost, myPort);
        }

        public void Dispose()
        {
            if (_client != null) _client.Close();
        }

        public void Send(string msg)
        {
            Byte[] data = Encoding.ASCII.GetBytes(msg);
            Array.Resize(ref data, data.Length + 1);
            data[data.Length - 1] = (byte)0x1E;
            NetworkStream stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
            //_client.Close();
        }
    }
}
