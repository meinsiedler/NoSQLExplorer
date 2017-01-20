using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DatabaseInteraction.Queries
{
  public class GetAverageFollowersQuery : IQuery<double>
  {
    public GetAverageFollowersQuery()
    {
    }

    public GetAverageFollowersQuery(string hashtag)
    {
      if (!hashtag.StartsWith("#"))
        throw new ArgumentException("Hashtag must start with '#'");

      Hashtag = hashtag;
    }

    public string Hashtag { get; }
  }
}
