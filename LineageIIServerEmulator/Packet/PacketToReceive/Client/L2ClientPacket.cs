using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.PacketToReceive.Client
{
  public abstract class L2ClientPacket : ReceivablePacket
  {

    public L2ClientPacket(L2Client Client, byte[] Packet) : base(Client, Packet)
    {
    }
  }
}
