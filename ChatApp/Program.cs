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
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-server":
                        if (args.Length >=2)
                        {
                            int port = -1;
                            bool portParsed = int.TryParse(args[1], out port);

                            if (!portParsed)
                            {
                                throw new Exception("Argument 2 must be a port number. The given argument was not a number.");
                            }
                            Server server = new Server(port);
                            server.Start();
                        }
                        else
                        {
                            throw new Exception("You are missing the 2nd argument. Please provide the port to connect to.");
                        }
                        break;

                    case "-client":
                        if (args.Length >= 3)
                        {
                            IPAddress ipConnect;
                            bool ipParsed = IPAddress.TryParse(args[1], out ipConnect);
                            if (!ipParsed)
                                throw new Exception("Argument should be an IP Address. The given argument was not a IP Address/Invalid IP Address");

                            int port = -1;
                            bool portParsed = int.TryParse(args[2], out port);
                            if (!portParsed)
                            {
                                throw new Exception("Argument 3 must be a port number. The given argument was not a number");
                            }
                            Client client = new Client(ipConnect, port);
                            client.Start();
                        }
                        else
                        {
                            throw new Exception("Too many arguments. Order of arguments is as follows IP:Port (ex. 123.45.67.89:9876)");
                        }
                        break;
                }



                /*
                if (args[0] == "-server")
                {
                    int port = 0;

                    try
                    {
                        bool portParsed = int.TryParse(args[1], out port);

                        if (!portParsed)
                        {
                            throw new Exception("Argument 3 must be a port number. The given argument was not a number");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

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

                    if (portParsed)
                    {
                        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                    }
                    else
                    {
                        throw new Exception("Argument 3 must be a port number. The given argument was not a number");
                    }
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
                    IPAddress ipAddress = args[1];
                    bool ipParsed = IPAddress.TryParse(args[1], out ipAddress);
                    if (!ipParsed)
                        throw new Exception("Argument should be an IP Address. The given argument was not a IP Address");

                    int port = 0;
                    bool portParsed = int.TryParse(args[2], out port);

                    if (!portParsed)
                    {
                        throw new Exception("Argument 3 must be a port number. The given argument was not a number");
                    }
                    //Socket is responsible for the connection between the server & client
                    Socket socket;

                    socket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );


                    Console.WriteLine("Connecting To The Server...");
                    socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 420));
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
                                //
                                /*
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
                }*/
            }
            else
            {
                Console.WriteLine("Please proviode agruments to this application type either -server or -client");
            }

        }
    }
}
