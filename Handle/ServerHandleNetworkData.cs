namespace ServerCore.Handle
{
    internal class ServerHandleNetworkData
    {
        // Use this for initialization
        private delegate void Packet_(int index, byte[] data);

        private static Dictionary<int, Packet_> Packets;
        //Every Packet send by the client must Have an integer to know The Type of The message
        /* exp :
         * When the client join the room He need to send with the packet (ClientPackets.CJoinRoom)
         * */

        public static void HandlNetworkInformation(int index, byte[] data)
        {
            try
            {
                int packetnum;
                PacketBuffer buffer = new PacketBuffer();
                buffer.WriteBytes(data);
                packetnum = buffer.ReadInteger();
                buffer.Dispose();

                Packet_ packet_;
                if (Packets.TryGetValue(packetnum, out packet_))

                {
                    packet_.Invoke(index, data);
                }
            }
            catch
            {
            }
        }

        public static void InitializeNetworkPackages()
        {
           Console.WriteLine("Initialize Network Packages");

            //Packets Dictionary who Handle all The messages from the client
            /**exp :
             *  Add new Handle :
             *  -Add new enum in ClientPackets(same as the client)
             *  -Add { (int)ClientPackets.Exp, HandleExp}, in the Dictionary Packets
             *
             * */
            Packets = new Dictionary<int, Packet_>
        {
             { (int)ClientPackets.CCmd, Handlecmd},
               { (int)ClientPackets.CSendMessage, HandleChat},


        };
        }

        private static void HandleChat(int index, byte[] data)
        {
            try
            {


                PacketBuffer buffer = new PacketBuffer();
                buffer.WriteBytes(data);
                int packetnum = buffer.ReadInteger();
                string sender = buffer.ReadString();
                string msg = buffer.ReadString();

                Console.WriteLine(msg + " Has Send  msg  " + msg);
                buffer.Dispose();
               ServerSendData.instance.sendMessageChatToAll(msg, sender);


            }
            catch
            {

            }
        }

        private static void Handlecmd(int index, byte[] data)
        {
            try
            {


                PacketBuffer buffer = new PacketBuffer();
                buffer.WriteBytes(data);
                int packetnum = buffer.ReadInteger();
                int cmd = buffer.ReadInteger();
                string msg = buffer.ReadString();

                Console.WriteLine(msg + " Has Send  cmd  "+cmd);
                buffer.Dispose();
                ServerSendData.instance.SendCmdToAll(msg, cmd);

   
            }
            catch
            {

            }
        }
    }
}