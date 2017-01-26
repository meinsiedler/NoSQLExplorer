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

    private readonly Stream stream;

    private byte[] buffer = new byte[512];
    private CancellationTokenSource cts = new CancellationTokenSource();

    public StreamWatcher(Stream stream)
    {
      if (stream == null)
      {
        throw new ArgumentNullException(nameof(stream));
      }

      this.stream = stream;
    }

    protected void OnMessageAvailable(MessageReceivedEventArgs e)
    {
      MessageReceived?.Invoke(this, e);
    }

    protected async void WatchAsync()
    {
      while (!this.cts.IsCancellationRequested)
      {
        var bytesRead = await stream.ReadAsync(buffer, 0, 512); // TODO: can a message be more than 512 bytes long?
        if (bytesRead == 0)
        {
          await Task.Delay(250);
        }
        else
        {
          int specialChars = 0;
          var message = await Task.Run(() => Encoding.ASCII.GetString(buffer.Where(c =>
          {
            if (IsSpecialChar(c))
            {
              specialChars++;
              return false;
            }

            return true;
          }).ToArray(), 0, bytesRead - specialChars));

          this.OnMessageAvailable(new MessageReceivedEventArgs(bytesRead, message));
        }
      }
    }

    private bool IsSpecialChar(byte c)
    {
      return (c == 0) || (c == 1);
    }

    public void Start()
    {
      if (this.cts.IsCancellationRequested)
      {
        throw new InvalidOperationException("stream was already cancelled");
      }

      WatchAsync();
    }

    public void Stop()
    {
      this.cts.Cancel();
    }
  }
}
