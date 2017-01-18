using System.Collections.Generic;
using System.Threading.Tasks;
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
      return Task.FromResult(true);
    }

    public Task BulkInsertAsync(IList<Tweet> tweets)
    {
      return Task.FromResult(true);
    }
  }
}
