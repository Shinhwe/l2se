using LineageIIServerEmulator.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DAO
{
    public class AccountDAO
    {
        public static AccountDAO _Instance;
        private AccountDAO()
        {

        }
        public static AccountDAO GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new AccountDAO();
            }
            return _Instance;
        }
        public bool CheckPassword(string UserName, string PassWord)
        {
            SqlConnection conn;
            DataTable dt = new DataTable();
            try
            {
                conn = L2Database.GetInstance().GetConnection();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM account WHERE login = @UserName AND password = @PassWord";
                    SqlParameter[] Params = new SqlParameter[]
                    {
                        new SqlParameter("@UserName",UserName),
                        new SqlParameter("@PassWord", PassWord)
                    };
                    cmd.Parameters.AddRange(Params);
                    using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dt.Load(sdr);
                    }
                }
                return dt.Columns.Count > 0;
            }
            catch (Exception e)
            {
                //TODO: log error
            }
            return false;
        }

        public bool AddNewAccount(string UserName, string PassWord)
        {
            return false;
        }

        public bool FindAccount(string UserName, string PassWord)
        {
            return false;
        }

        public bool FindAccountByName(string UserName)
        {
            return false;
        }
    }
}
