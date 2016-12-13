using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoSqlExplorer.TwitterReader.Configuration;
using NoSqlExplorer.TwitterReader.Model;

namespace NoSqlExplorer.TwitterReader
{
  public class TwitterReader : ITwitterReader
  {
    private readonly object _lock = new object();
    private readonly ITwitterConfigSettings _configSettings;
    private CancellationTokenSource _cancellationTokenSource;

    public TwitterReader(ITwitterConfigSettings configSettings)
    {
      _configSettings = configSettings;
    }

    public async Task StartAsync(string accessToken, string accessTokenSecret)
    {
      _cancellationTokenSource = new CancellationTokenSource();
      var token = _cancellationTokenSource.Token;


      var oauth = new OAuth(_configSettings.TwitterConsumerKey, _configSettings.TwitterConsumerSecret);
      var nonce = oauth.Nonce();
      var timestamp = oauth.TimeStamp();
      var signature = oauth.Signature("GET", _configSettings.TwitterFeedUrl, nonce, timestamp, accessToken, accessTokenSecret, parameters: Enumerable.Empty<string[]>());
      var authorizeHeader = oauth.AuthorizationHeader(nonce, timestamp, accessToken, signature);

      var request = (HttpWebRequest)WebRequest.Create(_configSettings.TwitterFeedUrl);
      request.Headers.Add("Authorization", authorizeHeader);
      request.Method = "GET";

      using (var response = await request.GetResponseAsync())
      {
        await Task.Factory.StartNew(() =>
        {
          lock (_lock)
          {
            IsRunning = true;
          }

          using (var streamReader = new StreamReader(response.GetResponseStream()))
          {
            string line;
            while (!token.IsCancellationRequested && (line = streamReader.ReadLine()) != null)
            {
              var closureLine = line;
              Task.Run(() => OnNewTweet(closureLine)); // call event on different thread so that event subscribers do not block the reading of subsequent lines
            }
          }

          lock (_lock)
          {
            IsRunning = false;
          }
        }, token);
      }
    }

    public void Stop()
    {
      _cancellationTokenSource.Cancel();
    }

    public event Action<string> OnNewTweet = delegate { };
    public bool IsRunning { get; private set; }
  }
}
