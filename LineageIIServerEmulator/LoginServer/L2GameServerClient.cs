﻿using LineageIIServerEmulator.LoginServer.Crypt;
using LineageIIServerEmulator.Packet;
using LineageIIServerEmulator.Packet.Handler;
using LineageIIServerEmulator.Packet.ServerPacket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.Packet.ServerPacket.LoginServerPacket;
using System.Net;

namespace LineageIIServerEmulator.LoginServer
{
  public class L2GameServerClient
  {
    private Socket _Conn;
    private byte[] Buffer = new byte[8192];

    public L2GameServerClient(Socket Conn)
    {
      _Conn = Conn;
    }
  }
}
