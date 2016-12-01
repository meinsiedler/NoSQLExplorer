using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.TwitterReader
{
  public interface ITwitterReader
  {
    void Start();
    void Stop();

    event Action<string> OnNewTweet;

    bool IsRunning { get; }
  }
}
