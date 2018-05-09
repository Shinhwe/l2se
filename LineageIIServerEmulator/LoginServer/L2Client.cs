using LineageIIServerEmulator.Utils.Crypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using LineageIIServerEmulator.Packet;
using LineageIIServerEmulator.Packet.Handler;
using LineageIIServerEmulator.Packet.PacketToSend.Client;

namespace LineageIIServerEmulator.LoginServer
{
  public class L2Client
  {
    private Socket _Conn;
    private int _SessionId;
    private byte[] _PublicKey;
    private byte[] _BlowfishKey;
    private ScrambledKeyPair ScrambledPair;
    private LoginCrypt Crypt;
    private byte[] Buffer = new byte[8192];
    private LoginSession _Session;
    private string ClientIp;
    private bool _Closed = false;



    public L2Client(Socket Conn)
    {
      _Conn = Conn;
      _SessionId = Conn.GetHashCode();
      ScrambledPair = new ScrambledKeyPair();
      _PublicKey = ScrambledPair.GetScrambledModulus();
      GenerateBlowfishKey();
      Crypt = new LoginCrypt(_BlowfishKey);
      Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
      ClientIp = ((IPEndPoint)_Conn.RemoteEndPoint).Address.ToString();
      SendPacket(new Init(this));
    }

    public void SetLoginSession(LoginSession Session)
    {
      _Session = Session;
    }

    public Org.BouncyCastle.Crypto.CipherParameters GetPrivateKey()
    {
      return ScrambledPair.GetPrivateKey();
    }

    public bool CheckLogin(int Id1, int Id2)
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
    public void SendPacket(SendablePacket Packet)
    {
      byte[] PacketBytes = Packet.GetBytes();
      using (Packet)
      {
        PacketBytes = Crypt.Encrypt(PacketBytes);
        using (MemoryStream s = new MemoryStream())
        {
          byte[] PacketLength = BitConverter.GetBytes((short)(PacketBytes.Length + 2));
          s.Write(PacketLength, 0, PacketLength.Length);
          s.Write(PacketBytes, 0, PacketBytes.Length);
          PacketBytes = s.ToArray();
        }
      }
      _Conn.Send(PacketBytes);
    }

    private void GenerateBlowfishKey()
    {
      byte[] BlowfishKey = new byte[16];
      new Random(DateTime.Now.Millisecond).NextBytes(BlowfishKey);
      _BlowfishKey = BlowfishKey;
    }
    private void AynsReceive(IAsyncResult result)
    {
      if (!_Closed)
      {
        if (_Conn.Connected == false)
        {
          return;
        }
        int BytesTransferred = _Conn.EndReceive(result);
        _Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
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
          ClientPacketHandler Handler = new ClientPacketHandler(this, Packet);
        }
      }
    }
    public void DisConnetion(LoginFailReason Reason)
    {
      SendPacket(new LoginFail(Reason));
      _Conn.Close();
      _Closed = true;
    }
  }
}
