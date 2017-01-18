using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Twitter.Common
{
  public class Tweet
  {
    public Tweet() { }

    public Tweet(long id, string text, string source, long userId, int followers, DateTime timestamp)
    {
      Id = id;
      Text = text;
      Source = source;
      UserId = userId;
      Followers = followers;
      Timestamp = timestamp;
    }

    public long Id { get; set; }
    public string Text { get; set; }
    public string Source { get; set; }
    public long UserId { get; set; }
    public int Followers { get; set; }
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
      return $"Id: {Id}, Text: {Text?.Substring(0, 20) ?? "-"}, Timestamp: {Timestamp}";
    }
  }
}
