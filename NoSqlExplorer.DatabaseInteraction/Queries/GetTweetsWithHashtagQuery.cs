using System;
using System.Collections.Generic;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction.Queries
{
  public class GetTweetsWithHashtagQuery : IQuery<IList<Tweet>>
  {
    public string Hashtag { get; }

    public GetTweetsWithHashtagQuery(string hashtag)
    {
      if(!hashtag.StartsWith("#"))
        throw new ArgumentException("Hashtag must start with '#'");

      Hashtag = hashtag;
    }
  }
}
