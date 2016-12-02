using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DockerAdapter.Util
{
  public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

  public class MessageReceivedEventArgs : EventArgs
  {
    public MessageReceivedEventArgs(int messageSize, string message) : base()
    {
      this.Message = message;
    }

    public string Message { get; private set; }
    public int MessageSize { get; private set; }
  }
}
