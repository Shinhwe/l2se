﻿using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.PacketToSend.Client
{
  public class LoginSuccess : L2LoginServerPacket
  {
    LoginSession _Session;
    public LoginSuccess(LoginSession Session)
    {
      _Session = Session;
    }
    protected override void writeImpl()
    {
      WriteByte(0x03);
      WriteInt(_Session.GetLoginId1());
      WriteInt(_Session.GetLoginId2());
      WriteInt(0x00);
      WriteInt(0x00);
      WriteInt(0x000003ea);
      WriteInt(0x00);
      WriteInt(0x00);
      WriteInt(0x00);
      WriteBytes(new byte[16]);
    }
  }
}
