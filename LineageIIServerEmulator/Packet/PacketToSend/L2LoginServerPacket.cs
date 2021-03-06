﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.PacketToSend
{
  public abstract class L2LoginServerPacket : SendablePacket
  {

    public L2LoginServerPacket()
    {
    }

    public override bool write()
    {
      try
      {
        writeImpl();
        return true;
      }
      catch (Exception e)
      {
        throw e;
      }
      return false;
    }

    protected abstract void writeImpl();
  }
}
