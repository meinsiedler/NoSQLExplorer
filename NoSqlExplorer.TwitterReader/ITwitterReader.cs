using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.TwitterReader
{
  public interface ITwitterReader
  {
    Task StartAsync(string accessToken, string accessTokenSecret);
    void Stop();

    event Action<Tweet> OnNewTweet;

    bool IsRunning { get; }
  }
}
