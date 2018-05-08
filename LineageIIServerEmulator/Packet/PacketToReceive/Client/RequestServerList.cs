using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.LoginServer;

namespace LineageIIServerEmulator.Packet.ClientPacket.LoginClientPacket
{
  public class RequestServerList : L2ClientPacket
  {
    private int _loginOkId1;
    private int _loginOkId2;
    public RequestServerList(L2LoginClient Client, byte[] Packet) : base(Client, Packet)
    {
      handler();
    }

    protected override void readImpl()
    {
      _loginOkId1 = ReadInt();
      _loginOkId2 = ReadInt();
    }

    protected override void handlerImpl()
    {
      if (_Client.CheckLogin(_loginOkId1, _loginOkId2))
      {
        //TODO: ServerList
        //_Client.SendPacket(new ServerList());
      }
    }
  }
}
