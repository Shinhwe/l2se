using LineageIIServerEmulator.LoginServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DataBase
{
  public class SQLHelper
  {
    private SqlConnection conn = null;
    private SqlCommand cmd = null;
    private SqlDataReader sdr = null;
    private static SQLHelper _Instance;
    private string _ConnStr = "";

    /// <summary>
    /// 构造方法,用于构造一个SQLHerlper实例,并初始化数据库连接
    /// </summary>
    private SQLHelper()
    {
      _ConnStr = @"data source=" + LoginConfig.DATA_BASE_HOST + "," + LoginConfig.DATA_BASE_PORT + ";initial catalog=" + LoginConfig.DATA_BASE_NAME + ";user id=" + LoginConfig.DATA_BASE_USER + ";password=" + LoginConfig.DATA_BASE_PASSWORD + ";MultipleActiveResultSets=True";
    }

    public static SQLHelper GetInstance()
    {
      if (_Instance == null)
      {
        _Instance = new SQLHelper();
      }
      return _Instance;
    }

    /// <summary>
    /// 该方法用于打开数据库连接
    /// </summary>
    /// <returns>打开连接的数据库,异常时返回空值</returns>
    public SqlConnection getConn()
    {
      try
      {
        conn = new SqlConnection(_ConnStr);
      }
      catch (Exception e)
      {
        throw e;
      }
      return conn;
    }

    /// <summary>
    /// 该方法用于对数据库执行增/删/改操作
    /// </summary>
    /// <param name="cmdText">需要执行的SQL语句</param>
    /// <param name="paras">SQL参数(可选)</param>
    /// <param name="ct">命令类型</param>
    /// <returns>执行结果</returns>
    public bool ExecuteNonQuery(string cmdText, params SqlParameter[] paras)
    {
      int res = -1;
      using (SqlConnection con = getConn())
      {
        try
        {
          using (cmd = new SqlCommand(cmdText, con))
          {
            cmd.CommandType = CommandType.Text;
            if (paras != null)
            {
              cmd.Parameters.AddRange(paras);
            }
            res = cmd.ExecuteNonQuery();
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        finally
        {
          con.Close();
        }
      }
      return res > 0;
    }

    /// <summary>
    /// 该方法用于对数据库进行查询操作
    /// </summary>
    /// <param name="cmdText">需要执行的SQL语句</param>
    /// <param name="paras">SQL参数(可选)</param>
    /// <param name="ct">命令类型</param>
    /// <returns>执行结果</returns>
    public DataTable ExecuteQuery(string cmdText, params SqlParameter[] paras)
    {
      DataTable dt = new DataTable();
      using (SqlConnection con = getConn())
      {
        try
        {
          cmd = new SqlCommand(cmdText, con);
          cmd.CommandType = CommandType.Text;
          if (paras != null)
          {
            cmd.Parameters.AddRange(paras);
          }
          using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            dt.Load(sdr);
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        finally
        {
          con.Close();
        }
      }
      return dt;
    }
  }
}
