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
                            int port = 0;
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
                            throw new Exception("You are missing the 2nd argument. " +
                            "Please provide the port to connect to.");
                        }
                        break;

                    case "-client":
                        if (args.Length >= 3)
                        {
                            IPAddress ipConnect;
                            bool ipParsed = IPAddress.TryParse(args[1], out ipConnect);
                            if (!ipParsed)
                                throw new Exception("Argument should be an IP Address. The given argument was not a IP Address/Invalid IP Address");

                            int port = 0;
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
                            throw new Exception("Order of arguments is as follows 'client IP Port' (ex. -client 123.45.67.89 9876)");
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please proviode agruments to this application type either -server or -client");
            }

        }
    }
}
