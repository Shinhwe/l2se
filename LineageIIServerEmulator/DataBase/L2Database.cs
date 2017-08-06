using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DataBase
{
    public class L2Database
    {
        private static L2Database _Instance;
        private string ConnStr = "";
        private L2Database()
        {
            ConnStr = @"data source=.;initial catalog="+LoginConfig.DATA_BASE_NAME+ ";user id=" + LoginConfig.DATA_BASE_USER + ";password=" + LoginConfig.DATA_BASE_PASSWORD + ";MultipleActiveResultSets=True";
        }

        public static L2Database GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new L2Database();
            }
            return _Instance;
        }

        public SqlConnection GetConnection()
        {

            try
            {
                SqlConnection Conn = new SqlConnection(ConnStr);
                Conn.Open();
                return Conn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Close(SqlConnection Conn)
        {
            try
            {
                Conn.Close();
            }
            catch (Exception)
            {
                //be quite
            }
        }

    }
}
