using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction
{
  internal class MongoDatabaseInteractor : IDatabaseInteractor
  {
    public MongoDatabaseInteractor(string containerName, string host)
    {
      ContainerName = containerName;
      Host = host;
    }

    public string ContainerName { get; }
    public string Host { get; }

    public Task EnsureTableExistsAsync()
    {
      // TODO
      return Task.FromResult(true);
    }

    public Task BulkInsertAsync(IList<Tweet> tweets)
    {
      // TODO
      return Task.FromResult(true);
    }

    public Task<IList<Tweet>> GetQueryResultAsync(GetTweetsWithHashtagQuery query)
    {
      // TODO
      return Task.FromResult((IList<Tweet>)new List<Tweet>());
    }

    public Task<double> GetQueryResultAsync(GetAverageFollowersQuery query)
    {
      // TODO
      return Task.FromResult(0d);
    }
  }
}
