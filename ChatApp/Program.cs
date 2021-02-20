using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //github > profile > SSH & GPG > Generate SSH key on google > copy command and change the email > enter command & find ssh > c drive > users > ssh > open public key copy everything inside > go to github > new ssh key paste ssh & give a name.
            //edit the repo on the visual control aplication (fork/st) test SSH key from the website.
            //git ignore + git repo 
            //===
            //to test this program open Cmder & enter the debug folder of this project & type the assemblyname.exe which here we set to "app"
            //open 2 Cmder windows and do "app.exe -server" first to start up the server then on the other Cmder window do "app.exe -client"
            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    //how to connect multiple clients 
                    List<Socket> clientSockets = new List<Socket>();

                    //Listening Socket
                    Socket listeningSocket;
                    listeningSocket = new Socket(
                        AddressFamily.InterNetwork,         //IPV4
                        SocketType.Stream,                  //Type Stream
                        ProtocolType.Tcp                    //Protocol TCP
                        );

                    //bind to the client
                    listeningSocket.Blocking = false;
                    listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 420));
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
                        //Communicating Socket / Client Socket

                        //Socket clientSocket = listeningSocket.Accept();
                        for (int i = 0; i < clientSockets.Count; i++)
                        {
                            try
                            {
                                Byte[] recieveBuffer = new byte[1024];
                                int received = clientSockets[i].Receive(recieveBuffer);
                                //Console.WriteLine(ASCIIEncoding.ASCII.GetString(recieveBuffer));

                                for (int j = 0; j < clientSockets.Count; j++)
                                {
                                    //string stringToSend = Console.ReadLine();
                                    if (i != j)
                                    {
                                        clientSockets[j].Send(recieveBuffer, received, SocketFlags.None);//(ASCIIEncoding.ASCII.GetBytes(stringToSend));
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
                else if (args[0] == "-client")
                {

                    //Socket is responsible for the connection between the server & client
                    Socket socket;

                    socket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );


                    Console.WriteLine("Connecting To The Server...");
                    socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 420));
                    socket.Blocking = false;
                    Console.WriteLine("Connected To The Server!");

                    Console.WriteLine("Enter your nick name");
                    string nick = Console.ReadLine();
                    nick += ": ";

                    Console.WriteLine("Type your message...");
                    string stringToSend = "";
                    while (true)
                    {
                        try
                        {
                            if (Console.KeyAvailable)
                            {
                                //string stringToSend = Console.ReadLine();
                                ConsoleKeyInfo key = Console.ReadKey();

                                if (key.Key == ConsoleKey.Enter)
                                {
                                    string message = nick + stringToSend;
                                    socket.Send(ASCIIEncoding.ASCII.GetBytes(stringToSend));
                                    stringToSend = "";
                                    Console.WriteLine();
                                }
                                else
                                {
                                    stringToSend += key.KeyChar;
                                }

                                /*
                                Console.WriteLine("Key pressed is " + key);
                                socket.Send(ASCIIEncoding.ASCII.GetBytes(stringToSend));
                                //socket.Send(ASCIIEncoding.ASCII.GetBytes("Hello Server!"));
                                */
                            }

                            Byte[] recieveBuffer = new byte[1024];
                            int received = socket.Receive(recieveBuffer);
                            string stringToPrint = ASCIIEncoding.ASCII.GetString(recieveBuffer);
                            stringToPrint = stringToPrint.Substring(0, received);
                            Console.WriteLine(stringToPrint);

                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode != SocketError.WouldBlock)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Please proviode agruments to this application type either -server or -client");
            }

        }
    }
}
