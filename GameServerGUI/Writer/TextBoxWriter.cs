using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameServerGUI.Writer
{
  public class TextBoxWriter : TextWriter
  {
    private TextBox _TextBox;
    private Dispatcher _Dispatcher;
    public TextBoxWriter(TextBox _TextBox, Dispatcher _Dispatcher)
    {
      this._TextBox = _TextBox;
      this._Dispatcher = _Dispatcher;
    }

    public override void Write(char value)
    {
      _Dispatcher.BeginInvoke(new Action(() =>
      {
        _TextBox.Text += value;
      }));
    }

    public override void Write(string value)
    {
      _Dispatcher.BeginInvoke(new Action(() =>
      {
        _TextBox.Text += value;
      }));
    }

    public override Encoding Encoding
    {
      get
      {
        return Encoding.UTF8;
      }
    }
  }
}
