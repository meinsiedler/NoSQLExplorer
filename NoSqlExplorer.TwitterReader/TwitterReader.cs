using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoSqlExplorer.Twitter.Common;
using NoSqlExplorer.TwitterReader.Configuration;
using NoSqlExplorer.TwitterReader.Model;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.TwitterReader
{
  public class TwitterReader : ITwitterReader
  {
    private readonly string _twitterConsumerKey;
    private readonly string _twitterConsumerSecret;
    private readonly string _twitterFeedUrl;
    private readonly object _lock = new object();
    
    private CancellationTokenSource _cancellationTokenSource;

    public TwitterReader(string twitterConsumerKey, string twitterConsumerSecret, string twitterFeedUrl)
    {
      _twitterConsumerKey = twitterConsumerKey;
      _twitterConsumerSecret = twitterConsumerSecret;
      _twitterFeedUrl = twitterFeedUrl;
    }

    public async Task StartAsync(string accessToken, string accessTokenSecret)
    {
      _cancellationTokenSource = new CancellationTokenSource();
      var token = _cancellationTokenSource.Token;


      var oauth = new OAuth(_twitterConsumerKey, _twitterConsumerSecret);
      var nonce = oauth.Nonce();
      var timestamp = oauth.TimeStamp();
      var signature = oauth.Signature("GET", _twitterFeedUrl, nonce, timestamp, accessToken, accessTokenSecret, parameters: Enumerable.Empty<string[]>());
      var authorizeHeader = oauth.AuthorizationHeader(nonce, timestamp, accessToken, signature);

      var request = (HttpWebRequest)WebRequest.Create(_twitterFeedUrl);
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
              if (line.StartsWith("{\"created_at\""))
              {
                var closureLine = line;
                Task.Run(async () => OnNewTweet(await ParseTweetAsync(closureLine))); // call event on different thread so that event subscribers do not block the reading of subsequent lines
              }
            }
          }

          lock (_lock)
          {
            IsRunning = false;
          }
        }, token);
      }
    }

    private async Task<Tweet> ParseTweetAsync(string line)
    {
      var json = await Task.Run(() => (JObject)JsonConvert.DeserializeObject(line));
      var id = long.Parse(json["id"].ToString());
      var text = json["text"].ToString();
      var source = json["source"].ToString();
      var userId = long.Parse(json["user"].SelectToken("id").ToString());
      var timestamp = TimestampConverter.TimestampToDateTime(long.Parse(json["timestamp_ms"].ToString()));
      var tweet = new Tweet(id, text, source, userId, timestamp);
      return tweet;
    }

    public void Stop()
    {
      _cancellationTokenSource.Cancel();
    }

    public event Action<Tweet> OnNewTweet = delegate { };
    public bool IsRunning { get; private set; }
  }
}
