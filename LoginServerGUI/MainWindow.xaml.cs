using LineageIIServerEmulator.LoginServer;
using LoginServerGUI.Writer;
using System;
using System.Threading.Tasks;

namespace LoginServerGUI
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      Console.SetOut(new TextBoxWriter(OutputTextBox));
      Console.SetError(new TextBoxWriter(OutputTextBox));

      new Task(delegate { LoginServer.GetInstance(); }).Start();
    }
  }
}
