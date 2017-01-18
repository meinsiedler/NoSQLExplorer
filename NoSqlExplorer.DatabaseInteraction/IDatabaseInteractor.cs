using System.Collections.Generic;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction
{
  public interface IDatabaseInteractor
  {
    string ContainerName { get; }
    string Host { get; }

    Task EnsureTableExistsAsync();
    Task BulkInsertAsync(IList<Tweet> tweets);

    Task<IList<Tweet>> GetQueryResultAsync(GetTweetsWithHashtagQuery query);
    Task<double> GetQueryResultAsync(GetAverageFollowersQuery query);
  }
}
