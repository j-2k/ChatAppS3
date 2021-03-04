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
        List<Socket> clientSockets;
        Dictionary<Socket, string> clientSocketNickname;
        Socket listeningSocket;
        int port;

        public Server(int port)
        {
            this.port = port;
            clientSockets = new List<Socket>();
            clientSocketNickname = new Dictionary<Socket, string>();
        }

        public void Start()
        {
            //Listening Socket
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
                    Socket clientSocket = listeningSocket.Accept();
                    Byte[] recieveBuffer = new byte[1024];
                    int receivedBytes = clientSocket.Receive(recieveBuffer);
                    string nickname = ASCIIEncoding.ASCII.GetString(recieveBuffer);
                    nickname = nickname.Substring(0, receivedBytes);
                    Console.WriteLine(nickname + " has connected to the server!");

                    clientSocketNickname.Add(clientSocket,nickname);
                    clientSockets.Add(clientSocket);
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
                            //SENDING TO SERVER
                            Socket disconnectedSocket = clientSockets[i];
                            string nickname = clientSocketNickname[disconnectedSocket];
                            Console.WriteLine(nickname + " Disconnected!");

                            clientSockets[i].Close();
                            clientSocketNickname.Remove(disconnectedSocket);
                            clientSockets.RemoveAt(i);

                            //SENDING TO CLIENTS
                            Packet disconnectedPacket = new Packet();
                            disconnectedPacket.nickname = "Server";
                            disconnectedPacket.message = nickname + " Disconnected!";
                            disconnectedPacket.textColor = ConsoleColor.Red;

                            for (int j = 0; j < clientSockets.Count; j++)
                            {
                                clientSockets[j].Send(BinaryFormatterClass.ObjectToByteArray(disconnectedPacket)); //??
                            }
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
