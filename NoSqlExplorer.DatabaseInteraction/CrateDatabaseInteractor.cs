using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.DAL.Common;
using NoSqlExplorer.Twitter.Common;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DatabaseInteraction
{
  internal class CrateDatabaseInteractor : IDatabaseInteractor
  {
    [TableName("tweets")]
    internal struct CrateTweet
    {
      public CrateTweet(Tweet tweet) : this()
      {
        Id = tweet.Id;
        Text = tweet.Text;
        Source = tweet.Source;
        UserId = tweet.UserId;
        Timestamp = tweet.Timestamp;
      }

      [PrimaryKey]
      public long Id { get; }
      public string Text { get; }
      public string Source { get; }
      public long UserId { get; }
      public DateTime Timestamp { get; }
    }

    private readonly string _crateUrl;
    private readonly int? _shards;

    public CrateDatabaseInteractor(string containerName, string host, string crateUrl, int? shards)
    {
      ContainerName = containerName;
      Host = host;
      _crateUrl = crateUrl;
      _shards = shards;
    }

    public string ContainerName { get; }
    public string Host { get; }

    public async Task EnsureTableExistsAsync()
    {
      var crateClient = new CrateClient(_crateUrl);
      await crateClient.CreateTable<CrateTweet>(_shards);
    }

    public async Task BulkInsertAsync(IList<Tweet> tweets)
    {
      var crateClient = new CrateClient(_crateUrl);
      await Retry.TryAwait<ICrateResponse, HttpRequestException>(() => crateClient.BulkInsert(tweets.Select(t => new CrateTweet(t))));
    }
  }
}
