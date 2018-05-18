using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Net;

namespace LineageIIServerEmulator.LoginServer
{
  public class L2ClientNetServer : NetServer
  {
    public L2ClientNetServer()
    {
      Name = "ClientNetServer";
      ProtocolType = NetType.Tcp;
      Port = 2106;
    }
  }
}
