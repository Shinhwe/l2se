using LineageIIServerEmulator.Packet;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LineageIIServerEmulator.LoginServer
{
  public class LoginServer
  {
    private static LoginServer _Instace;
    private Socket _LoginServerSocket;
    private Socket _LoginSocket;
    L2Client Client;
    L2GameServerClient GameServerClient;
    private readonly ILog Log;
    private LoginServer()
    {
      ILoggerRepository LoginServerRepository = LogManager.CreateRepository("LoginServerRepository");
      XmlConfigurator.Configure(LoginServerRepository, new FileInfo("./config/loginserver/log4net.config"));
      Log = LogManager.GetLogger("LoginServerRepository", typeof(LoginServer));
      LoginConfig.Load();
      Utils.Crypt.ScrambledKeyPair.Init();
      InitLoginServer();
    }
    public static LoginServer GetInstance()
    {
      if (_Instace == null)
      {
        _Instace = new LoginServer();
      }
      return _Instace;
    }
    private void InitLoginServer()
    {
      try
      {
        IPEndPoint LoginServerIP = new IPEndPoint(IPAddress.Parse(LoginConfig.LOGIN_SERVER_HOST), LoginConfig.LOGIN_SERVER_PORT);
        _LoginServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _LoginServerSocket.Bind(LoginServerIP);
        _LoginServerSocket.Listen(100);
        _LoginServerSocket.BeginAccept(LoginServerAction, null);
        Log.Info("开始监听" + LoginConfig.LOGIN_SERVER_PORT + "端口...等待连接...");

        IPEndPoint LoginIP = new IPEndPoint(IPAddress.Parse(LoginConfig.LOGIN_HOST), LoginConfig.LOGIN_PORT);
        _LoginSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _LoginSocket.Bind(LoginIP);
        _LoginSocket.Listen(100);
        _LoginSocket.BeginAccept(LoginAction, null);
        Log.Info("开始监听" + LoginConfig.LOGIN_PORT + "端口...等待连接...");
      }
      catch (Exception e)
      {
				Console.WriteLine(e);
        //TODO: log error
      }
    }

    private void LoginAction(IAsyncResult result)
    {
      Socket AcceptSocket = _LoginSocket.EndAccept(result);
      _LoginSocket.BeginAccept(LoginAction, null);
      GameServerClient = new L2GameServerClient(AcceptSocket);
    }

    private void LoginServerAction(IAsyncResult result)
    {
      Socket AcceptSocket = _LoginServerSocket.EndAccept(result);
      _LoginServerSocket.BeginAccept(LoginServerAction, null);
      Client = new L2Client(AcceptSocket);
    }
  }
}
