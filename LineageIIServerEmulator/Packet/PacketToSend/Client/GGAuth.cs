using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.PacketToSend.Client
{
  public class GGAuth : L2LoginServerPacket
  {
    private int _SessionId;
    public GGAuth(int SessionId)
    {
      _SessionId = SessionId;
    }
    protected override void writeImpl()
    {
      WriteByte(0x0b);
      WriteInt(_SessionId);
      WriteInt(0x00);
      WriteInt(0x00);
      WriteInt(0x00);
      WriteInt(0x00);
    }
  }
}
