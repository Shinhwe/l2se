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
    public static string LOGIN_HOST;
    public static int LOGIN_PORT;
    public static string DATA_BASE_HOST;
    public static int DATA_BASE_PORT;
    public static string DATA_BASE_NAME;
    public static string DATA_BASE_USER;
    public static string DATA_BASE_PASSWORD;
    public static bool AUTO_CREATE_ACCOUNT;
    public static string ACCOUNT_TEMPLATE;
    public static string PASSWORD_TEMPLATE;

    public static void Load()
    {
      LoadConfiguration();
    }
    private static void LoadConfiguration()
    {
      Properties ServerSettings = LoadConfig.Load("./Config/LoginServer/Server.ini");
      LOGIN_SERVER_HOST = ServerSettings.GetProperty("LoginServerHost", "0.0.0.0");
      LOGIN_SERVER_PORT = ServerSettings.GetProperty("LoginServerPort", 2106);
      LOGIN_HOST = ServerSettings.GetProperty("LoginHost", "127.0.0.1");
      LOGIN_PORT = ServerSettings.GetProperty("LoginPort", 9014);
      DATA_BASE_HOST = ServerSettings.GetProperty("DataBaseHost", ".");
      DATA_BASE_HOST = ServerSettings.GetProperty("DataBasePort", "1433");
      DATA_BASE_NAME = ServerSettings.GetProperty("DataBaseName", "l2se");
      DATA_BASE_USER = ServerSettings.GetProperty("DataBaseUser", "sa");
      DATA_BASE_PASSWORD = ServerSettings.GetProperty("DataBasePassWord", "123456");
      ACCOUNT_TEMPLATE = ServerSettings.GetProperty("AccountTemplate", "123456");
      PASSWORD_TEMPLATE = ServerSettings.GetProperty("PasswordTemplate", "123456");
      AUTO_CREATE_ACCOUNT = ServerSettings.GetProperty("AutoCreateAccount", true);
    }
  }
}
