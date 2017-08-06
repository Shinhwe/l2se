using LineageIIServerEmulator.Packet.ServerPacket.LoginServerPacket;
using System;
using System.Collections.Generic;
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
        private int _Port;
        private IPAddress _Host;
        private Socket _LoginServerSocket;
        L2LoginClient Client;
        private LoginServer()
        {
            LoginConfig.Load();
            InitLoingServer();
        }
        public static LoginServer GetInstance()
        {
            if (_Instace == null)
            {
                _Instace = new LoginServer();
            }
            return _Instace;
        }
        private void InitLoingServer()
        {
            try
            {
                _Host = IPAddress.Parse(LoginConfig.LOGIN_SERVER_HOST);
                _Port = LoginConfig.LOGIN_SERVER_PORT;
                IPEndPoint ipe = new IPEndPoint(_Host, _Port);
                _LoginServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _LoginServerSocket.Bind(ipe);
                _LoginServerSocket.Listen(100);
                _LoginServerSocket.BeginAccept(AsyncAction, null);
                Console.WriteLine("开始监听" + _Port + "端口...等待连接...");
            }
            catch (Exception e)
            {
                //TODO: log error
            }
        }

        private void AsyncAction(IAsyncResult result)
        {
            Socket AcceptSocket = _LoginServerSocket.EndAccept(result);
            Client = new L2LoginClient(AcceptSocket);
            Client.SendPacket(new Init(Client));
            _LoginServerSocket.BeginAccept(AsyncAction, null);
        }
    }
}
