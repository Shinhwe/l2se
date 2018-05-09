using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.LoginServer;
using LineageIIServerEmulator.Packet.PacketToSend.Client;

namespace LineageIIServerEmulator.Packet.PacketToReceive.Client
{
  public class AuthGameGuard : L2ClientPacket
  {
    private int _SessionId;
    public AuthGameGuard(L2Client Client, byte[] Packet) : base(Client, Packet)
    {
      handler();
    }
    protected override void readImpl()
    {
      _SessionId = ReadInt();
    }

    protected override void handlerImpl()
    {
      if (_Client.GetSessionId() == _SessionId)
      {
        _Client.SendPacket(new GGAuth(_SessionId));
      }
    }
  }
}
