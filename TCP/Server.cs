using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SKKLib.Console.SKKConsole;

namespace SKKLib.TCP
{
    public class Server
    {
        public delegate void ServerReceivedMessage_EH(string s);
        public event ServerReceivedMessage_EH ServerReceivedMessage = delegate { };
        public delegate void NewConnection_EH();
        public event NewConnection_EH NewConnection = delegate { };

        private readonly byte delim = (byte)0x1e;

        private TcpListener listener = null;
        private TcpClient myClient = null;
        private NetworkStream myStream = null;
        private Thread listenThread;
        public bool Listening { get; private set; } = false;

        public Server(IPAddress ip, int port)
        {
            listener = new TcpListener(ip, port);
        }

        public void Start()
        {
            if (Listening) return;

            DBG("Starting Listen Thread");
            Listening = true;
            listener.Start();

            listenThread = new Thread(ListenHandler);
            listenThread.Start();
        }

        public void Stop() => Listening = false;

        private void ListenHandler()
        {
            while (Listening)
            {
                try
                {
                    // If we have an incoming connection and we haven't made our client yet,
                    // then create our client and connect...
                    // If we have an incoming connection but we've already made our client,
                    // then tough tittie for that incoming connection...
                    if ((listener.Pending()) && (myClient == null))
                    {
                        DBG("Creating 'myClient'");
                        myClient = listener.AcceptTcpClient();
                        myStream = myClient.GetStream();
                        NewConnection();
                    }

                    // If we've made our client, then process
                    if (myClient != null)
                    {
                        DBG("Processing 'myClient' NetworkStream");
                        Byte[] buffer = new Byte[1024];
                        string data = String.Empty;
                        StringBuilder sb = new StringBuilder();
                        int bytesRead;
                        int index;

                        // Read data from stream
                        while((bytesRead = myStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            // Look for a delimeter
                            while ((index = Array.IndexOf(buffer, delim)) != -1)
                            {
                                // If we found one, extract from start of buffer to delim index as a new message
                                sb.Append(Encoding.ASCII.GetString(buffer, 0, index));
                                
                                // Fire message off to whoever is handling it
                                ServerReceivedMessage(sb.ToString());
                                
                                // Clear string builder of old data
                                sb.Clear();
                                
                                // Remove just processed message & delim from buffer
                                buffer = buffer.Skip(index + 1).ToArray();
                                
                                // Adjust how many bytes we've read in to not include processed message & delim
                                bytesRead -= (index + 1);
                            }
                            
                            // What's left in the buffer is either nothing or the start of a new message, so add it to sb
                            sb.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                        }
                    }
                }
                catch (SocketException)
                {
                    Debugger.Break();
                }
                catch (IOException)
                {
                    // Client disconnected... 
                    DBG("'myClient' disconnected...");
                    myClient = null;
                }
            }
            DBG("Listen Thread Ending");
        }
    }
}
