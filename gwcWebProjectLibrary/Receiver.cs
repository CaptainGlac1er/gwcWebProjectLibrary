using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gwcWebProjectLibrary
{
    class Receiver
    {
        Connection connection;
        Thread recieverThread;
        Queue<byte[]> dataToSend;
        bool running = false;
        Socket socket;
        public Receiver(Connection connection, Socket socket)
        {
            this.socket = socket;
            this.connection = connection;
            dataToSend = new Queue<byte[]>();
            recieverThread = new Thread(new ThreadStart(receive));

        }
        public void startListening()
        {
            running = true;
            recieverThread.Start();
        }
        public void receive()
        {
            while (true)
            {
                byte[] dataIn = new byte[1024];
                socket.Receive(dataIn);
                connection.inData(dataIn);
            }
        }
        public void stopListening()
        {
            running = false;
        }

    }
}
