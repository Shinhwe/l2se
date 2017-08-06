using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.LoginServer;
using LineageIIServerEmulator.Packet.ServerPacket.LoginServerPacket;

namespace LineageIIServerEmulator.Packet.ClientPacket.LoginClientPacket
{
    public class AuthGameGuard : L2ClientPacket
    {
        private int _SessionId;
        public AuthGameGuard(L2LoginClient Client, byte[] Packet) : base(Client, Packet)
        {
            handler();
        }
        protected override void readImpl()
        {
            _SessionId = ReadInt();
        }

        protected override void handlerImpl()
        {
            if (_Client.GetSessionId() == _SessionId)
            {
                _Client.SendPacket(new GGAuth(_SessionId));
            }
        }
    }
}
