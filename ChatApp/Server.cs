using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    class Server //app.exe -server 420
    {
        List<Socket> clientSockets = new List<Socket>();
        int port;

        public Server(int port)
        {
            this.port = port;
            clientSockets = new List<Socket>();
        }

        public void Start()
        {
            //Listening Socket
            Socket listeningSocket;
            listeningSocket = new Socket(
                AddressFamily.InterNetwork,         //IPV4
                SocketType.Stream,                  //Type Stream-2way connection
                ProtocolType.Tcp                    //Protocol TCP
                );

            listeningSocket.Blocking = false;

            //bind to the client
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            Console.WriteLine("Waiting For Connection...");
            listeningSocket.Listen(10); //listen

            while (true)
            {
                try //keep trying to listen
                {
                    clientSockets.Add(listeningSocket.Accept());
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                    {
                        Console.WriteLine(ex);
                    }
                }

                for (int i = 0; i < clientSockets.Count; i++)
                {
                    try
                    {
                        Byte[] recieveBuffer = new byte[1024];
                        int received = clientSockets[i].Receive(recieveBuffer);

                        for (int j = 0; j < clientSockets.Count; j++)
                        {
                            if (i != j)
                            {
                                clientSockets[j].Send(recieveBuffer, received, SocketFlags.None);
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionAborted ||
                            ex.SocketErrorCode == SocketError.ConnectionReset)
                        {
                            clientSockets[i].Close();
                            clientSockets.RemoveAt(i);
                        }

                        if (ex.SocketErrorCode != SocketError.WouldBlock)
                        {
                            if (ex.SocketErrorCode != SocketError.ConnectionAborted ||
                                ex.SocketErrorCode != SocketError.ConnectionReset)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
            }


        }
    }
}
