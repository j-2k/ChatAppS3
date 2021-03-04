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
    class Client //app.exe -client 127.0.0.1 420
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
                            socket.Send(ASCIIEncoding.ASCII.GetBytes(message));
                            Console.WriteLine(nick + stringToSend);
                            message = "";
                            stringToSend = "";
                            //Console.WriteLine();
                        }
                        else
                        {
                            if(key.Key == ConsoleKey.Backspace)
                            {
                                if (stringToSend.Length >= 1)
                                {
                                    stringToSend = stringToSend.Remove(stringToSend.Length - 1, 1);
                                    Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}\r");
                                    Console.Write($"\r{stringToSend}");
                                }
                            }
                            else
                            {
                                stringToSend += key.KeyChar;
                            }
                        }
                    }

                    //RECIEVING THE MESSAGE

                    Byte[] recieveBuffer = new byte[1024];
                    int receivedBytes = socket.Receive(recieveBuffer);
                    string stringToPrint = ASCIIEncoding.ASCII.GetString(recieveBuffer);
                    stringToPrint = stringToPrint.Substring(0, receivedBytes);
                    Console.Write($"\r{new string(' ',(Console.WindowWidth - 1))}\r");
                    Console.WriteLine(stringToPrint);
                    Console.WriteLine(stringToSend);
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
