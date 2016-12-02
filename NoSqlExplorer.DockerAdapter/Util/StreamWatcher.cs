using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoSqlExplorer.DockerAdapter.Util
{
  public class StreamWatcher
  {
    public event MessageReceivedEventHandler MessageReceived;

    private bool read;
    private readonly Stream stream;

    private byte[] buffer = new byte[512];
    private CancellationTokenSource cts;

    public StreamWatcher(Stream stream, System.Threading.CancellationTokenSource cts = null)
    {
      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      this.stream = stream;
      this.cts = cts;
    }

    protected void OnMessageAvailable(MessageReceivedEventArgs e)
    {
      MessageReceived?.Invoke(this, e);
    }

    protected async void WatchNext()
    {
      if (!read)
      {
        return;
      }
      var bytesRead = await stream.ReadAsync(buffer, 0, 512);
      if (bytesRead == 0)
      {
        await Task.Delay(250);
      }
      else
      {
        int specialChars = 0;
        var message = Encoding.ASCII.GetString(buffer.Where(c =>
        {
          if (!IsSpecialChar(c))
          {
            return true;
          }

          specialChars++;
          return false;
        }).ToArray(), 0, bytesRead - specialChars);
        this.OnMessageAvailable(new MessageReceivedEventArgs(bytesRead, message));
      }

      WatchNext();
    }

    private bool IsSpecialChar(byte c)
    {
      return (c == 0) || (c == 1);
    }

    public void Start()
    {
      if (this.cts != null && this.cts.IsCancellationRequested)
      {
        throw new InvalidOperationException("stream was already cancelled");
      }

      this.read = true;
      WatchNext();
    }

    public void Stop()
    {
      this.read = false;
      this.cts?.Cancel();
    }
  }
}
