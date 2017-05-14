using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace gwcWebProjectLibrary.Client
{
    class Client
    {
        Socket connectionSocket;
        public Client()
        {

        }
        public void connect(string ip, int port)
        {
            IPHostEntry host = Dns.GetHostEntry(ip);
            foreach (IPAddress address in host.AddressList)
            {
                try
                {
                    IPEndPoint ipe = new IPEndPoint(address, port);
                    Console.WriteLine("trying " + address.ToString());
                    connectionSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    connectionSocket.Connect(ipe);
                    if (connectionSocket.Connected)
                    {
                        Console.WriteLine("trying " + address.ToString() + " succed");
                        break;
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("trying " + address.ToString() + " failed");
                    continue;
                }

            }
        }
        public void sendData(byte[] data)
        {
            connectionSocket.Send(data);
        }
    }
}
