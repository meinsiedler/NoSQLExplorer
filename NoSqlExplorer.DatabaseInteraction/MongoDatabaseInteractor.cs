using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction
{
  internal class MongoDatabaseInteractor : IDatabaseInteractor
  {
    private MongoDbClient client;

    public MongoDatabaseInteractor(string containerName, string host, int port, string username, string password)
    {
      ContainerName = containerName;
      Host = host;
      this.client = new MongoDbClient(host, port, username, password, "TweetCollection");
    }

    public string ContainerName { get; }
    public string Host { get; }

    public Task EnsureTableExistsAsync()
    {
      return Task.FromResult(true);
    }

    public async Task BulkInsertAsync(IList<Tweet> tweets)
    {
      await this.client.AddAsync(tweets.AsEnumerable());
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
