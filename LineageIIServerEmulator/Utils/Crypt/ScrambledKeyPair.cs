using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace LineageIIServerEmulator.Utils.Crypt
{
  public class ScrambledKeyPair
  {
    private static readonly ILog Log = LogManager.GetLogger(typeof(ScrambledKeyPair));
    public AsymmetricCipherKeyPair _pair;
    public byte[] _scrambledModulus;
    private static AsymmetricCipherKeyPair[] _KeyPairs = new AsymmetricCipherKeyPair[10];

    private static AsymmetricCipherKeyPair GeneratorKeyPair() 
    {
      SecureRandom rnd = new SecureRandom();
      RsaKeyGenerationParameters par = new RsaKeyGenerationParameters(BigInteger.ValueOf(65537), rnd, 1024, 10);
      RsaKeyPairGenerator gen = new RsaKeyPairGenerator();
      gen.Init(par);
      AsymmetricCipherKeyPair keys = gen.GenerateKeyPair();
      return keys;
    }
    
    public static void Init()
    {
      Log.Info("开始缓存RSA密钥");
      for (var i = 0; i < 10; i++) 
      {
        _KeyPairs[i] = GeneratorKeyPair();
      }
      Log.Info($"缓存[{_KeyPairs.Length}]个RSA密钥");
    }

    public ScrambledKeyPair()
    {
      _pair = _KeyPairs[new Random().Next(0, 10)];
      _scrambledModulus = scrambleModulus(((RsaKeyParameters)_pair.Public).Modulus);
    }

    private byte[] scrambleModulus(BigInteger modulus)
    {
      byte[] scrambledMod = modulus.ToByteArray();

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

    public byte[] GetScrambledModulus()
    {
      return _scrambledModulus;
    }

    public AsymmetricKeyParameter GetPrivateKey()
    {
      return _pair.Private;
    }

    public AsymmetricKeyParameter GetPublicKey()
    {
      return _pair.Public;
    }
  }
}
