/*
 * ServerPackets && ClientPackets must be the same in the Server and in the Client
 * 
 * */

//get send from server to client
//client has to listen to serverpackets
public enum ServerPackets
{

    Scmd = 1,
    SSendMessage = 2,

}


//get send from client to server
//server has to listen to clientpackets
public enum ClientPackets
{
    CCmd = 1,
    CSendMessage = 2,


}