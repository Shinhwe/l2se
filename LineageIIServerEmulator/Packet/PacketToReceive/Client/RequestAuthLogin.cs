﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineageIIServerEmulator.LoginServer;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using LineageIIServerEmulator.Packet.ServerPacket.LoginServerPacket;
using LineageIIServerEmulator.LoginServer.Models;
using System.Text.RegularExpressions;

namespace LineageIIServerEmulator.Packet.ClientPacket.LoginClientPacket
{
  public class RequestAuthLogin : L2ClientPacket
  {
    private byte[] _Raw = new byte[128];
    public RequestAuthLogin(L2LoginClient Client, byte[] Packet) : base(Client, Packet)
    {
      handler();
    }
    protected override void readImpl()
    {
      _Raw = ReadBytes(_Raw.Length);
      // read this for what ?
      //readD();
      //readD();
      //readD();
      //readD();
      //readD();
      //readD();
      //readH();
      //readC();
    }

    protected override void handlerImpl()
    {
      string UserName = "";
      string PassWord = "";
      CipherParameters PrivateKey = _Client.GetPrivateKey();
      RSAEngine rsa = new RSAEngine();
      rsa.init(false, PrivateKey);
      byte[] decrypt = rsa.processBlock(_Raw, 0, 128);
      if (decrypt.Length < 128)
      {
        byte[] temp = new byte[128];
        Array.Copy(decrypt, 0, temp, 128 - decrypt.Length, decrypt.Length);
        decrypt = temp;
      }
      UserName = PrepareString(Encoding.UTF8.GetString(decrypt, 0x5E, 14).ToLower());
      PassWord = PrepareString(Encoding.UTF8.GetString(decrypt, 0x6C, 16));
      if (Regex.Match(UserName, LoginConfig.ACCOUNT_TEMPLATE).Success && Regex.Match(PassWord, LoginConfig.PASSWORD_TEMPLATE).Success)
      {
        Account AC = new Account(UserName, PassWord);
        if (AC.IsExist())
        {
          if (AC.IsPassWordCorrect())
          {
            LoginSession Session = new LoginSession();
            _Client.SetLoginSession(Session);
            _Client.SendPacket(new LoginSuccess(Session));
          }
          else
          {
            _Client.DisConnetion(LoginFailReason.REASON_USER_OR_PASS_WRONG);
          }
        }
        else
        {
          if (LoginConfig.AUTO_CREATE_ACCOUNT)
          {
            AC.Register();
            LoginSession Session = new LoginSession();
            _Client.SetLoginSession(Session);
            _Client.SendPacket(new LoginSuccess(Session));
          }
          else
          {
            _Client.DisConnetion(LoginFailReason.REASON_ACCOUNT_INFO_INCORR);
          }
        }
      }
      else
      {
        _Client.DisConnetion(LoginFailReason.REASON_USER_OR_PASS_WRONG);
      }

    }
    private string PrepareString(string Value)
    {
      StringBuilder SB = new StringBuilder();
      for (short i = 0; i < Value.Length - 1; i++)
      {
        if (char.IsLetterOrDigit(Value[i]))
        {
          SB.Append(Value[i]);
        }
      }
      return SB.ToString();
    }

  }
}
