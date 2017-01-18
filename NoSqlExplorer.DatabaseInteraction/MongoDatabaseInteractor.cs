using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.DatabaseInteraction.QueryHandlers.Mongo;
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

    public async Task<IList<Tweet>> GetQueryResultAsync(GetTweetsWithHashtagQuery query)
    {
      var handler = new GetTweetsWithHashtagQueryHandler(this.client);
      return await handler.HandleAsync(query);
    }

    public async Task<double> GetQueryResultAsync(GetAverageFollowersQuery query)
    {
      var handler = new GetAverageFollowersQueryHandler(this.client);
      return await handler.HandleAsync(query);
    }
  }
}
