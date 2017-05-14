using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gwcWebProjectLibrary
{
    class Sender
    {
        Connection connection;
        Thread senderThread;
        Queue<byte[]> dataToSend;
        public Sender(Connection connection)
        {
            this.connection = connection;
            dataToSend = new Queue<byte[]>();
            senderThread = new Thread(new ThreadStart(sendData));

        }
        public void sendData(byte[] data)
        {
            lock (dataToSend)
            {
                
                dataToSend.Enqueue(data);
            }
            if(senderThread.ThreadState != ThreadState.Running)
            {
                senderThread = new Thread(new ThreadStart(sendData));
                senderThread.Start();
            }
        }
        public void sendData()
        {
            while(dataToSend.Count > 0)
            {
                byte[] current;
                lock (dataToSend)
                {
                    current = dataToSend.Dequeue();
                }
                connection.sendData(current);
            }
        }

    }
}
