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
        string nickname;

        public Client(IPAddress ipC,int portC,string nicknameC)
        {
            this.ipToConnect = ipC;
            this.portToConnect = portC;
            this.nickname = nicknameC;
        }

        public void Start()
        {
            //Socket is responsible for the connection between the server & client
            Socket socket;
            Packet packetSend = new Packet();

            packetSend.nickname = nickname;

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

            /*Console.WriteLine("Enter your nick name");
            packetSend.nickname = Console.ReadLine();*/

            socket.Send(ASCIIEncoding.ASCII.GetBytes(nickname));


            int colorNumber = ChatColorsClass.PrintGetColorInfoFromUser();
            packetSend.textColor = ChatColorsClass.GetIndexOfColor(colorNumber);

            Console.WriteLine("Type your message...");

            while (true)
            {
                try
                {
                    if (Console.KeyAvailable)
                    {
                        Console.ForegroundColor = packetSend.textColor;
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter)
                        {
                            packetSend.message = packetSend.nickname + ": " + packetSend.message;
                            socket.Send(BinaryFormatterClass.ObjectToByteArray(packetSend));
                            Console.WriteLine(packetSend.message);
                            packetSend.message = "";
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Backspace)
                            {
                                if (packetSend.message.Length >= 1)
                                {
                                    packetSend.message = packetSend.message.Remove(packetSend.message.Length - 1, 1);
                                    Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}\r");
                                    Console.Write($"\r{packetSend.message}");
                                }
                            }
                            else
                            {
                                packetSend.message += key.KeyChar;
                            }
                        }
                    }

                    //RECIEVING THE MESSAGE

                    Byte[] recieveBuffer = new byte[1024];
                    int receivedBytes = socket.Receive(recieveBuffer);
                    Packet packetRecieve = (Packet)BinaryFormatterClass.ByteArrayToObject(recieveBuffer);

                    Console.Write($"\r{new string(' ',(Console.WindowWidth - 1))}\r");

                    Console.ForegroundColor = packetRecieve.textColor;
                    Console.WriteLine(packetRecieve.message);

                    Console.ForegroundColor = packetSend.textColor;
                    Console.WriteLine(packetSend.message);
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
