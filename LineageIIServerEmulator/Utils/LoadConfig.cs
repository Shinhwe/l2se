using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Utils
{
  public static class LoadConfig
  {
    public static Properties Load(string ConfigFile)
    {
      Properties P = new Properties();
      using (StreamReader SR = File.OpenText(ConfigFile))
      {
        string Line;
        while ((Line = SR.ReadLine()) != null)
        {
          Line = Line.Trim();
          if (Line.StartsWith("#") || Line.StartsWith("//"))
          {
            continue;
          }
          string[] ConfigLine = Line.Split('=');
          if (ConfigLine.Length == 2)
          {
            string ConfigName = ConfigLine[0].Trim();
            string ConfigValue = ConfigLine[1].Trim();
            P.AddProperty(ConfigName, ConfigValue);
          }
        }
      }
      return P;
    }
  }
  public class Properties : IDisposable
  {
    private Dictionary<string, string> _Properties = new Dictionary<string, string>();

    public string GetProperty(string Key, string DefaultValue)
    {
      if (_Properties.ContainsKey(Key))
      {
        return _Properties[Key];
      }
      return DefaultValue;
    }

    public bool GetProperty(string Key, bool DefaultValue)
    {
      if (_Properties.ContainsKey(Key))
      {
        return Convert.ToBoolean(_Properties[Key]);
      }
      return DefaultValue;
    }

    public long GetProperty(string Key, long DefaultValue)
    {
      if (_Properties.ContainsKey(Key))
      {
        return Convert.ToInt64(_Properties[Key]);
      }
      return DefaultValue;
    }

    public double GetProperty(string Key, double DefaultValue)
    {
      if (_Properties.ContainsKey(Key))
      {
        return Convert.ToDouble(_Properties[Key]);
      }
      return DefaultValue;
    }

    public int GetProperty(string Key, int DefaultValue)
    {
      if (_Properties.ContainsKey(Key))
      {
        return Convert.ToInt32(_Properties[Key]);
      }
      return DefaultValue;
    }

    public void AddProperty(string Key, string Value)
    {
      if (_Properties.ContainsKey(Key))
      {
        _Properties[Key] = Value;
      }
      else
      {
        _Properties.Add(Key, Value);
      }
    }

    public void Dispose()
    {
      _Properties = null;
    }
  }
}
