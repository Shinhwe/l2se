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
        private int _Port = 2106;
        private IPAddress _Host = IPAddress.Parse("0.0.0.0");
        private Socket _LoginServerSocket;
        L2LoginClient Client;
        private LoginServer()
        {
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
            IPEndPoint ipe = new IPEndPoint(_Host, _Port);
            _LoginServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _LoginServerSocket.Bind(ipe);
            _LoginServerSocket.Listen(100);
            _LoginServerSocket.BeginAccept(AsyncAction, null);
            Console.WriteLine("开始监听2106端口...等待连接...");
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
