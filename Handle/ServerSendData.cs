using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Handle
{
    internal class ServerSendData
    {
        public static ServerSendData instance;
        public ServerSendData()
        {
            instance = this;
          

        }
        /// <summary>
        /// Send data to specific client 
        /// </summary>
        /// <param name="index">Index client</param>
        /// <param name="data">data to send</param>
        public void SendDataTo(int index, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);

            sizeinfo[3] = (byte)(data.Length >> 24);
            SocketServer.instance._clients[index].socket.Send(sizeinfo);
            SocketServer.instance._clients[index].socket.Send(data);

        }
        /// <summary>
        /// Send data to a list of clients
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="index">list index of clients</param>
        public void SendDataToList(byte[] data, List<int> index)
        {
            try
            {


                foreach (var item in index)
                {

                    if (SocketServer.instance._clients[item].socket != null)
                    {
                        SendDataTo(SocketServer.instance._clients[item].index, data);


                    }
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// Send data to all client connect to the server
        /// </summary>
        /// <param name="data">Data to send</param>
        public void SendDataToAll(byte[] data)
        {
            try
            {


                for (int i = 0; i < 100; i++)
                {

                    if (SocketServer.instance._clients[i].socket != null)
                    {
                        SendDataTo(SocketServer.instance._clients[i].index, data);


                    }
                }
            }
            catch
            {

            }

        }
        public void sendMessageChatToAll(string sender,string msg)
        {

            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SSendMessage);
            buffer.WriteString(sender);

            buffer.WriteString(msg);


            SendDataToAll(buffer.ToArray());
            buffer.Dispose();
        }
        public void SendCmdToAll(string sender,int cmd)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.Scmd);
            buffer.WriteInteger(cmd);

            buffer.WriteString(sender);


            SendDataToAll(buffer.ToArray());
            buffer.Dispose();

        }
        public void SendConnectionOk(int index)
        {
          /*  PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("");


            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
          */
        }

    }
}
