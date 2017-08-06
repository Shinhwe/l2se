using LineageIIServerEmulator.LoginServer.Crypt;
using LineageIIServerEmulator.Packet;
using LineageIIServerEmulator.Packet.Handler.LoginServer;
using LineageIIServerEmulator.Packet.ServerPacket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.LoginServer
{
    public class L2LoginClient
    {
        private Socket _Conn;
        private int _SessionId;
        private byte[] _PublicKey;
        private byte[] _BlowfishKey;
        private ScrambledKeyPair ScrambledPair;
        private LoginCrypt Crypt;
        private byte[] Buffer = new byte[8192];
        private LoginSession _Session;

        public L2LoginClient(Socket Conn)
        {
            _Conn = Conn;
            _SessionId = Conn.GetHashCode();
            ScrambledPair = new ScrambledKeyPair(ScrambledKeyPair.genKeyPair());
            _PublicKey = ScrambledPair.GetScrambledModulus();
            GenerateBlowfishKey();
            Crypt = new LoginCrypt();
            Crypt.updateKey(_BlowfishKey);
            Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
        }

        public void SetLoginSession(LoginSession Session)
        {
            _Session = Session;
        }

        public Org.BouncyCastle.Crypto.CipherParameters GetPrivateKey()
        {
            return ScrambledPair.GetPrivateKey();
        }

        public bool CheckLogin(int Id1,int Id2)
        {
            return _Session.GetLoginId1() == Id1 && _Session.GetLoginId2() == Id2;
        }

        public bool Decrypt(byte[] Bytes)
        {
            return Crypt.Decrypt(Bytes);
        }

        public int GetSessionId()
        {
            return _SessionId;
        }
        public byte[] GetPublicKey()
        {
            return _PublicKey;
        }
        public byte[] GetBlowfishKey()
        {
            return _BlowfishKey;
        }
        public void SendPacket(L2ServerPacket Packet)
        {
            byte[] PacketBytes = Packet.GetBytes();
            PacketBytes = Crypt.Encrypt(PacketBytes);
            MemoryStream s = new MemoryStream();
            byte[] PacketLength = BitConverter.GetBytes((short)(PacketBytes.Length + 2));
            s.Write(PacketLength, 0, PacketLength.Length);
            s.Write(PacketBytes, 0, PacketBytes.Length);
            _Conn.Send(s.ToArray());
        }

        private void GenerateBlowfishKey()
        {
            byte[] BlowfishKey = new byte[16];
            new Random(DateTime.Now.Millisecond).NextBytes(BlowfishKey);
            _BlowfishKey = BlowfishKey;
        }
        private void AynsReceive(IAsyncResult result)
        {
            int BytesTransferred = _Conn.EndReceive(result);
            if (BytesTransferred <= 0)
            {
                return;
            }
            int PacketLength = BitConverter.ToUInt16(Buffer, 0);
            PacketLength -= 2; //already read 2 bytes
            byte[] Packet = new byte[PacketLength];
            Array.Copy(Buffer, 2, Packet, 0, PacketLength);
            bool DecryptReult = Decrypt(Packet);
            if (DecryptReult)
            {
                LoginClientPacketHandler Handler = new LoginClientPacketHandler(this, Packet);
            }
            _Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
        }
    }
}
