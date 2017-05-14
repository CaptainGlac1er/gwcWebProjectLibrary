using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gwcWebProjectLibrary
{
    public class Connection
    {
        Thread listening, sending;
        IPEndPoint endPoint;
        Socket connectionSocket;
        Sender sender;
        Receiver receiver;
        gwcDataTransfer objectToUpdate;
        public Connection(gwcDataTransfer updater)
        {
            objectToUpdate = updater;
        }
        public void listen(int port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, port);
            connectionSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            connectionSocket.Bind(ipe);
            connectionSocket.Listen(1);
            connectionSocket = connectionSocket.Accept();
            startLink();
        }
        public void startLink()
        {
            sender = new Sender(this);
            receiver = new Receiver(this, connectionSocket);
            receiver.startListening();
        }
        public void start(String ip, int port)
        {
            IPHostEntry host = Dns.GetHostEntry(ip);
            if (host.AddressList.Length == 0)
            {
                try
                {
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port);
                    Console.WriteLine("trying " + ip.ToString());
                    connectionSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    connectionSocket.Connect(ipe);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("trying " + ip.ToString() + " failed");
                }
            }
            else
            {

                foreach (IPAddress address in host.AddressList)
                {
                    try
                    {
                        IPEndPoint ipe = new IPEndPoint(address, port);
                        Console.WriteLine("trying " + address.ToString());
                        connectionSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        connectionSocket.Connect(ipe);
                        if (connectionSocket.Connected)
                            break;
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("trying " + address.ToString() + " failed");
                        continue;
                    }

                }
            }
            startLink();
        }
        private void beginListener()
        {

        }
        public void gotConnection(Socket link)
        {
            connectionSocket = link;
        }
        public void queueToSend(byte[] data)
        {
            sender.sendData(data);
        }
        public bool sendData(byte[] data)
        {
            connectionSocket.Send(data);
            return true;
        }
        public void inData(byte[] data)
        {
            objectToUpdate.update(data);
        }
        
    }
}
