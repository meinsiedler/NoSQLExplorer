using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.TweetImporter
{
  internal class MongoTweetImporter : ITweetImporter
  {
    public Task EnsureTableExistsAsync()
    {
      return Task.FromResult(true);
    }

    public Task BulkInsertAsync(IList<Tweet> tweets)
    {
      return Task.FromResult(true);
    }
  }
}
