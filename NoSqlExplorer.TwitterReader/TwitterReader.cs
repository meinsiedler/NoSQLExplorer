using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoSqlExplorer.TwitterReader
{
  public class TwitterReader : ITwitterReader
  {
    private readonly object _lock = new object();
    private readonly string _apiUrl;
    private CancellationTokenSource _cancellationTokenSource;

    public TwitterReader(string apiUrl)
    {
      _apiUrl = apiUrl;
    }

    public void Start()
    {
      _cancellationTokenSource = new CancellationTokenSource();
      var token = _cancellationTokenSource.Token;

      var request = (HttpWebRequest)WebRequest.Create(_apiUrl);
      var response = (HttpWebResponse)request.GetResponse();

      Task.Factory.StartNew(() =>
      {
        lock (_lock)
        {
          IsRunning = true;
        }

        using (var stream = response.GetResponseStream())
        {
          using (var streamReader = new StreamReader(stream))
          {
            string line;
            while (!token.IsCancellationRequested && (line = streamReader.ReadLine()) != null)
            {
              var closureLine = line;
              Task.Run(() => OnNewTweet(closureLine)); // call event on different thread so that event subscribers do not block the reading of subsequent lines
            }
          }
        }

        lock (_lock)
        {
          IsRunning = false;
        }
      }, token);
    }

    public void Stop()
    {
      _cancellationTokenSource.Cancel();
    }

    public event Action<string> OnNewTweet = delegate { };
    public bool IsRunning { get; private set; }
  }
}
