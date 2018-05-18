using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LoginServerGUI.Writer
{
  public class TextBoxWriter : TextWriter
  {
    private TextBox _TextBox;
    public TextBoxWriter(TextBox _TextBox)
    {
      this._TextBox = _TextBox;
    }

    public override void Write(char value)
    {
      _TextBox.Dispatcher.Invoke(new Action(() =>
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
