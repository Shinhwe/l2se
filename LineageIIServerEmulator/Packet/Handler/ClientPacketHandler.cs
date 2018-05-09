using LineageIIServerEmulator.LoginServer;
using LineageIIServerEmulator.Packet.PacketToReceive.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.Handler
{
  public class ClientPacketHandler
  {
    private L2Client _Client;
    private byte[] _Packet;
    public ClientPacketHandler(L2Client Client, byte[] Packet)
    {
      _Client = Client;
      _Packet = Packet;
      HandlePacket();
    }
    public void HandlePacket()
    {
      int PacketId = _Packet[0];
      Console.WriteLine("接收到客户端封包: " + PacketId);
      switch (PacketId)
      {
        case 7:
          new AuthGameGuard(_Client, _Packet);
          break;
        case 0:
          new RequestAuthLogin(_Client, _Packet);
          break;
        case 5:
          new RequestServerList(_Client, _Packet);
          break;
      }
    }
  }
}
