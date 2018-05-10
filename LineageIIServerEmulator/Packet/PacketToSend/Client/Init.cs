using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.PacketToSend.Client
{
  public class Init : L2LoginServerPacket
  {
    private int _SessionId;

    private byte[] _PublicKey;
    private byte[] _BlowfishKey;
    public Init(L2Client Client)
    {
      _SessionId = Client.GetSessionId();
      _PublicKey = Client.GetPublicKey();
      _BlowfishKey = Client.GetBlowfishKey();
    }
    protected override void writeImpl()
    {
      WriteByte(0x00); // init packet id

      WriteInt(_SessionId); // session id
      WriteInt(0x0000c621); // protocol revision

      WriteBytes(_PublicKey); // RSA Public Key

      // unk GG related?
      WriteInt(0x29DD954E);
      WriteInt(0x77C39CFC);
      WriteInt((unchecked((int)0x97ADB620)));
      WriteInt(0x07BDE0F7);

      WriteBytes(_BlowfishKey); // BlowFish key
      WriteByte(0x00); // null termination ;)
    }
  }
}
