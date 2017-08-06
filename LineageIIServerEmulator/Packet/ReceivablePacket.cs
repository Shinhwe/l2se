using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet
{
    public abstract class ReceivablePacket
    {
        protected L2LoginClient _Client;
        protected byte[] _Packet;
        private int _OffSet = 1;

        public ReceivablePacket(L2LoginClient Client, byte[] Packet)
        {
            _Client = Client;
            _Packet = Packet;
        }

        public byte[] GetPacket()
        {
            return _Packet;
        }
        public byte ReadByte()
        {
            byte result = _Packet[_OffSet];
            _OffSet += 1;
            return result;
        }

        public byte[] ReadBytes(int Length)
        {
            byte[] result = new byte[Length];
            Array.Copy(_Packet, _OffSet, result, 0, Length);
            _OffSet += Length;
            return result;
        }

        public short ReadShort()
        {
            short result = BitConverter.ToInt16(_Packet, _OffSet);
            _OffSet += 2;
            return result;
        }

        public double ReadFloat()
        {
            double result = BitConverter.ToDouble(_Packet, _OffSet);
            _OffSet += 8;
            return result;
        }

        public int ReadInt()
        {
            int result = BitConverter.ToInt32(_Packet, _OffSet);
            _OffSet += 4;
            return result;
        }

        public string ReadString()
        {
            return "";
            //    StringBuilder SB = new StringBuilder();
            //    char ch;
            //    while ((ch = Encoding.UTF8.get)
            //        SB.Append(ch);
            //    return SB.ToString();
        }



        public abstract bool handler();
    }
}
