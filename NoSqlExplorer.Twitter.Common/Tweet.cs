using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Twitter.Common
{
  public struct Tweet
  {
    public Tweet(long id, string text, string source, long userId, DateTime timestamp)
    {
      Id = id;
      Text = text;
      Source = source;
      UserId = userId;
      Timestamp = timestamp;
    }

    public long Id { get; }
    public string Text { get; }
    public string Source { get; }
    public long UserId { get; }
    public DateTime Timestamp { get; }
  }
}
