using System.Collections.Generic;
using System.Threading.Tasks;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction
{
  public interface IDatabaseInteractor
  {
    string ContainerName { get; }
    string Host { get; }

    Task EnsureTableExistsAsync();
    Task BulkInsertAsync(IList<Tweet> tweets);
  }
}
