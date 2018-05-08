using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.LoginServer.Crypt
{
  public class ScrambledKeyPair
  {
    private AsymmetricCipherKeyPair _pair;
    private AsymmetricKeyParameter _publicKey;
    private byte[] _scrambledModulus;
    private AsymmetricKeyParameter _privateKey;

    public ScrambledKeyPair(AsymmetricCipherKeyPair pPair)
    {
      _pair = pPair;
      _publicKey = pPair.getPublic();
      _scrambledModulus = scrambleModulus((_publicKey as RSAKeyParameters).getModulus());
      _privateKey = pPair.getPrivate();
    }

    public AsymmetricKeyParameter GetPrivateKey()
    {
      return _privateKey;
    }
    public byte[] GetScrambledModulus()
    {
      return _scrambledModulus;
    }

    public static AsymmetricCipherKeyPair genKeyPair()
    {
      SecureRandom rnd = new SecureRandom();
      RSAKeyGenerationParameters par = new RSAKeyGenerationParameters(BigInteger.valueOf(65537), rnd, 1024, 10);
      RSAKeyPairGenerator gen = new RSAKeyPairGenerator();
      gen.init(par);
      AsymmetricCipherKeyPair keys = gen.generateKeyPair();
      return keys;
    }

    public byte[] scrambleModulus(BigInteger modulus)
    {
      byte[] fScrambledModulus = modulus.toByteArray();

      if (fScrambledModulus.Length == 0x81 && fScrambledModulus[0] == 0)
      {
        byte[] temp = new byte[0x80];
        Array.Copy(fScrambledModulus, 1, temp, 0, 0x80);
        fScrambledModulus = temp;
      }

      // step 1 0x4d-0x50  <-> 0x00-0x04
      for (int i = 0; i < 4; i++)
      {
        byte temp = fScrambledModulus[i];
        fScrambledModulus[i] = fScrambledModulus[0x4d + i];
        fScrambledModulus[0x4d + i] = temp;
      }

      // step 2   xor  first 0x40 bytes with  last 0x40 bytes
      for (int i = 0; i < 0x40; i++)
      {
        fScrambledModulus[i] = (byte)(fScrambledModulus[i] ^ fScrambledModulus[0x40 + i]);
      }

      // step 3  xor  bytes 0x0d-0x10 with bytes 0x34-0x38
      for (int i = 0; i < 4; i++)
      {
        fScrambledModulus[0x0d + i] = (byte)(fScrambledModulus[0x0d + i] ^ fScrambledModulus[0x34 + i]);
      }

      // step 4   xor  last 0x40 bytes with  first 0x40 bytes
      for (int i = 0; i < 0x40; i++)
      {
        fScrambledModulus[0x40 + i] = (byte)(fScrambledModulus[0x40 + i] ^ fScrambledModulus[i]);
      }

      return fScrambledModulus;
    }
  }
}
