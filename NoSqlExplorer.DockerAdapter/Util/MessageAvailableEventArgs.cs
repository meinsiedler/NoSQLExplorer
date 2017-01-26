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
    public MessageReceivedEventArgs(int messageSize, string message)
    {
      this.MessageSize = messageSize;
      this.Message = message;
    }

    public string Message { get; }
    public int MessageSize { get; }
  }
}
