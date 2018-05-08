using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.DataBase
{
  public class DataObject : Dictionary<string, object>
  {
    public int GetInt(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return Convert.ToInt32(this[Key]);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }
    public long GetLong(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return Convert.ToInt64(this[Key]);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }

    public bool GetBoolean(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return Convert.ToBoolean(this[Key]);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }

    public short GetTinyInt(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return Convert.ToInt16(this[Key]);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }

    public string GetString(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return this[Key].ToString();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }

    public double GetDouble(string Key)
    {
      if (ContainsKey(Key))
      {
        try
        {
          return Convert.ToDouble(this[Key]);
        }
        catch (Exception e)
        {
          throw e;
        }
      }
      throw new KeyNotFoundException();
    }
  }
}
