using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineageIIServerEmulator.Packet
{
  public abstract class SendablePacket : IDisposable
  {
    private MemoryStream VStream = new MemoryStream();
    public byte[] GetBytes()
    {
      return VStream.ToArray();
    }
    protected void WriteBytes(byte[] data)
    {
      VStream.Write(data, 0, data.Length);
    }

    protected void WriteByte(byte data)
    {
      VStream.WriteByte(data);
    }

    protected void WriteFloat(double value)
    {
      WriteBytes(BitConverter.GetBytes(value));
    }

    protected void WriteShort(short value)
    {
      WriteBytes(BitConverter.GetBytes(value));
    }

    protected void WriteInt(int value)
    {
      WriteBytes(BitConverter.GetBytes(value));
    }

    protected void WriteLong(long value)
    {
      WriteBytes(BitConverter.GetBytes(value));
    }

    protected void WriteString(string value)
    {
      if (value != null)
      {
        WriteBytes(Encoding.UTF8.GetBytes(value));
      }
      //WriteByte(0x00);
      //WriteByte(0x00);
    }

    public abstract bool write();


    public void Dispose()
    {
      VStream.Close();
      VStream.Dispose();
    }
  }
}
