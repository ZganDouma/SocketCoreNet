using ServerCore.Handle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Models
{
    internal class ClientModel
    {
        public int index;
        public string ip;
        public Socket socket;
        public bool Ready = false;
        public bool InGame = false;
        public bool Winner = false;
        bool isScore = false;
        public bool closing = false;
        //Size of the Message 
        private byte[] _buffer = new byte[16384];
        public bool CheckMeBool = true;
        internal bool alive;
        public void StartClient()
        {
            alive = true;
            CheckMeBool = true;
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            closing = false;
            isScore = false;

        }
        private void ReceiveCallback(IAsyncResult ar)
        {

            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);
                if (!socket.Connected)

                {
                    CloseClient(index);

                }
                else

                if (received <= 0)
                {
                    CloseClient(index);
                }
                else
                {
                    byte[] databuffer = new byte[received];
                    Array.Copy(_buffer, databuffer, received);
                    //This Function Handle all the The Messages that the client send
                    ServerHandleNetworkData.HandlNetworkInformation(index, databuffer);
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
            }
            catch (Exception ex)
            {

                CloseClient(index);

            }
        }
        public void CloseClient(int index)
        {
            socket.Close();
            SocketServer.instance._clients[index] = new ClientModel();
        }

        }
}
