using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoSqlExplorer.TwitterReader.Model;

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

    public async Task StartAsync(string accessToken, string accessTokenSecret)
    {
      _cancellationTokenSource = new CancellationTokenSource();
      var token = _cancellationTokenSource.Token;


      var oauth = new OAuth(accessToken, accessTokenSecret);
      var nonce = OAuth.Nonce();
      var timestamp = OAuth.TimeStamp();
      var signature = OAuth.Signature("GET", _apiUrl, nonce, timestamp, oauth.AccessToken, oauth.AccessTokenSecret, parameters: Enumerable.Empty<string[]>());
      var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, oauth.AccessToken, signature);

      var request = (HttpWebRequest)WebRequest.Create(_apiUrl);
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
