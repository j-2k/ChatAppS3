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
    class Client
    {
        IPAddress ipToConnect;
        int portToConnect;

        public Client(IPAddress ipC,int portC)
        {
            this.ipToConnect = ipC;
            this.portToConnect = portC;
        }

        public void Start()
        {
            //Socket is responsible for the connection between the server & client
            Socket socket;

            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
                );


            Console.WriteLine("Connecting To The Server...");
            //socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), 420));
            socket.Connect(new IPEndPoint(ipToConnect, portToConnect));
            Console.WriteLine("Connected To The Server!");
            socket.Blocking = false;

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
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter)
                        {
                            string message = nick + stringToSend;
                            socket.Send(ASCIIEncoding.ASCII.GetBytes(stringToSend));
                            message = "";
                            stringToSend = "";
                            Console.WriteLine();
                        }
                        else
                        {
                            stringToSend += key.KeyChar;
                        }
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
}
