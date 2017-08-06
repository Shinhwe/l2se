using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet.ClientPacket
{
    public abstract class L2ClientPacket : ReceivablePacket
    {

        public L2ClientPacket(L2LoginClient Client, byte[] Packet) : base(Client, Packet)
        {
        }

        public override bool handler()
        {
            try
            {
                readImpl();
                handlerImpl();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected abstract void readImpl();
        protected abstract void handlerImpl();
    }
}
