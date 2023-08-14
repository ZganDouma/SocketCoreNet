using ServerCore.Handle;
using ServerCore.Models;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    internal class SocketServer
    {
        /// <summary>
        /// 100 Max Clients per server can be more
        /// </summary>
        public ClientModel[] _clients = new ClientModel[100];

        public static SocketServer instance;
        public static string data = null;                                            //Stream             //Tcp
        public static Socket _serverSocket;

        //UDP
        private const int Port = 4800;

        private Socket server = new Socket(AddressFamily.InterNetwork,
        SocketType.Dgram, ProtocolType.Udp);

        private Thread thread;
        private bool running;
        private IPEndPoint AllEndPoint;

        public SocketServer()
        {
            instance = this;
            ServerHandleNetworkData.InitializeNetworkPackages();
            ServerSendData serverSendData = new ServerSendData();

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _serverSocket.ReceiveTimeout = 1000;
            _serverSocket.SendTimeout = 1000;
            /* Console.WriteLine(_serverSocket.ReceiveTimeout);
             Console.WriteLine(_serverSocket.SendTimeout);*/

            //Start Server TCP
            StartListening();

            //Start Server UDP
           // SendIp();
            running = true;
        }

        public void StartListening()
        {
            try
            {
               Console.WriteLine("Start listening");
                for (int i = 0; i < 100; i++)
                {
                    _clients[i] = new ClientModel();
                }
                //Accept Any Address in Port 5555
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5555));

                _serverSocket.Listen(100);

                //Start The Loop of Accept New Client
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Another server may be running on the same machine on port 5555 or 4800. Close them and restart the server.exe Thank you."
                    + "  " + ex.Message, false);
            }
        }
        private void AcceptCallBack(IAsyncResult ar)
        {

            try
            {


                Socket socket = _serverSocket.EndAccept(ar);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

                for (int i = 0; i < 100; i++)
                {
                    if (_clients[i].socket == null)
                    {
                        //Create New Client and Add the client to the list
                        _clients[i].socket = socket;
                        _clients[i].index = i;
                        _clients[i].ip = socket.RemoteEndPoint.ToString();
                        //Start Listner for every Client
                        _clients[i].StartClient();
                        Console.WriteLine("Connection from " + _clients[i].ip + " received");
            
                       // ServerSendData.instance.SendConnectionOk(i);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message, false);
            }
        }

    }
}