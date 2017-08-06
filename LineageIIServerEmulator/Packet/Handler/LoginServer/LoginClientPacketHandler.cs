using LineageIIServerEmulator.LoginServer;
using LineageIIServerEmulator.Packet.ClientPacket.LoginClientPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.Handler.LoginServer
{
    public class LoginClientPacketHandler
    {
        private L2LoginClient _Client;
        private byte[] _Packet;
        public LoginClientPacketHandler(L2LoginClient Client, byte[] Packet)
        {
            _Client = Client;
            _Packet = Packet;
            HandlePacket();
        }
        public void HandlePacket()
        {
            int PacketId = _Packet[0];
            Console.WriteLine("接收到客户端封包: " + PacketId);
            switch (PacketId)
            {
                case 7:
                    new AuthGameGuard(_Client, _Packet);
                    break;
                case 0:
                    new RequestAuthLogin(_Client, _Packet);
                    break;
                case 5:
                    new RequestServerList(_Client, _Packet);
                    break;
            }
        }
    }
}
