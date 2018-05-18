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
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using log4net;

namespace LineageIIServerEmulator.LoginServer
{
  public class L2Client
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof(L2Client));
    private Socket _Conn;
    private int _SessionId;
    private byte[] _PublicKey;
    private AsymmetricKeyParameter _PrivateKey;
    private byte[] _BlowfishKey;
    private ScrambledKeyPair ScrambledPair;
    private LoginCrypt Crypt;
    private byte[] Buffer = new byte[8192];
    private LoginSession _Session;
    private string ClientIp;
    private bool _Closed = false;

    public L2Client(Socket Conn)
    {
      Log.Info("接收到客户端连接!");
      _Conn = Conn;
      _SessionId = Conn.GetHashCode();
      ScrambledPair = new ScrambledKeyPair();
      _PublicKey = ScrambledPair.GetScrambledModulus();
      _PrivateKey = ScrambledPair.GetPrivateKey();
      GenerateBlowfishKey();
      Crypt = new LoginCrypt(_BlowfishKey);
      Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
      ClientIp = ((IPEndPoint)_Conn.RemoteEndPoint).Address.ToString();
      Log.Info("发送初始化封包!");
      SendPacket(new Init(this));
    }

    public void SetLoginSession(LoginSession Session)
    {
      _Session = Session;
    }

    public AsymmetricKeyParameter GetPrivateKey()
    {
      return _PrivateKey;
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
      Packet.write();
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
      Log.Info("接收到客户端发送信息");
      int BytesTransferred = _Conn.EndReceive(result);
      if (!_Closed)
      {
        if (BytesTransferred <= 0 ||　!_Conn.Connected)
        {
          _Conn.Close();
          _Closed = true;
          return;
        }
        _Conn.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AynsReceive, null);
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
    }

    private byte[] scrambleModulus(byte[] scrambledMod)
    {
      if ((scrambledMod.Length == 0x81) && (scrambledMod[0] == 0x00))
      {
        byte[] temp = new byte[0x80];
        Array.Copy(scrambledMod, 1, temp, 0, 0x80);
        scrambledMod = temp;
      }
      // step 1 : 0x4d-0x50 <-> 0x00-0x04
      for (int i = 0; i < 4; i++)
      {
        byte temp = scrambledMod[0x00 + i];
        scrambledMod[0x00 + i] = scrambledMod[0x4d + i];
        scrambledMod[0x4d + i] = temp;
      }
      // step 2 : xor first 0x40 bytes with last 0x40 bytes
      for (int i = 0; i < 0x40; i++)
      {
        scrambledMod[i] = (byte)(scrambledMod[i] ^ scrambledMod[0x40 + i]);
      }
      // step 3 : xor bytes 0x0d-0x10 with bytes 0x34-0x38
      for (int i = 0; i < 4; i++)
      {
        scrambledMod[0x0d + i] = (byte)(scrambledMod[0x0d + i] ^ scrambledMod[0x34 + i]);
      }
      // step 4 : xor last 0x40 bytes with first 0x40 bytes
      for (int i = 0; i < 0x40; i++)
      {
        scrambledMod[0x40 + i] = (byte)(scrambledMod[0x40 + i] ^ scrambledMod[i]);
      }

      return scrambledMod;
    }

  }
}
