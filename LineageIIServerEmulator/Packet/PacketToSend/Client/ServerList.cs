using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.ServerPacket.LoginServerPacket
{
  public class ServerList : L2LoginServerPacket
  {
    public ServerList()
    {
      write();
    }
    protected override void writeImpl()
    {

    }
  }
}
