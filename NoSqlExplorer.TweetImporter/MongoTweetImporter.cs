using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Mongo.DAL;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.TweetImporter
{
  internal class MongoTweetImporter : ITweetImporter
  {
    private MongoDbClient client;

    public MongoTweetImporter(string containerName, string host, int port, string username, string password)
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
      await this.client.AddAsync(tweets.FirstOrDefault());
    }
  }
}
