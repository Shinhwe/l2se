using LineageIIServerEmulator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.LoginServer
{
    public static class LoginConfig
    {
        public static string LOGIN_SERVER_HOST;
        public static int LOGIN_SERVER_PORT;
        public static string DATA_BASE_NAME;
        public static string DATA_BASE_USER;
        public static string DATA_BASE_PASSWORD;
        public static bool AUTO_CREATE_ACCOUNT;
        public static void Load()
        {
            LoadConfiguration();
        }
        public static void LoadConfiguration()
        {
            Properties ServerSettings = LoadConfig.Load("./Config/LoginServer/Server.ini");
            LOGIN_SERVER_HOST = ServerSettings.GetProperty("LoginServerHost", "0.0.0.0");
            LOGIN_SERVER_PORT = ServerSettings.GetProperty("LoginServerPort", 2106);
            DATA_BASE_NAME = ServerSettings.GetProperty("DataBaseName", "l2se");
            DATA_BASE_USER = ServerSettings.GetProperty("DataBaseUser", "sa");
            DATA_BASE_PASSWORD = ServerSettings.GetProperty("DataBasePassWord", "123456");
            AUTO_CREATE_ACCOUNT = ServerSettings.GetProperty("AutoCreateAccount", true);

        }
    }
}
