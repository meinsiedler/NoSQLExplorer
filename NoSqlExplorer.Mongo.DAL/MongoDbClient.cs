using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using NoSqlExplorer.DAL.Common;
using NoSqlExplorer.Mongo.DAL.Response;

namespace NoSqlExplorer.Mongo.DAL
{
  public class MongoDbClient
  {
    private MongoClient client;
    private IMongoDatabase database;

    public MongoDbClient(string hostname, int port, string username, string password, string databaseName)
    {
      var connectionString = this.BuildConnectionString(hostname, port, username, password);
      this.client = new MongoClient(connectionString);
      this.database = this.client.GetDatabase(databaseName);
    }

    public virtual async Task<IMongoResponse<T>> AddAsync<T>(T item)
    {
      try
      {
        await this.database.GetCollection<T>(Helper.GetTableName(typeof(T))).InsertOneAsync(item);
      }
      catch (Exception ex)
      {
        return new MongoResponse<T>(ex);
      }

      return new MongoResponse<T>(item);
    }

    public virtual async Task<IMongoResponse<T>> AddAsync<T>(IEnumerable<T> items)
    {
      try
      {
        await this.database.GetCollection<T>(Helper.GetTableName(typeof(T))).InsertManyAsync(items);
      }
      catch (Exception ex)
      {
        return new MongoResponse<T>(ex);
      }

      return new MongoResponse<T>(true);
    }

    public virtual async Task<IMongoResponse<T>> Single<T>(Expression<Func<T, bool>> expression)
    {
      try
      {

        var queryable = await Task.Run(() => this.database
          .GetCollection<T>(Helper.GetTableName(typeof(T)))
          .WithReadPreference(ReadPreference.PrimaryPreferred)
          .AsQueryable());

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = await queryable.SingleOrDefaultAsync(expression);
        stopWatch.Stop();

        return new MongoResponse<T>(result) { ExecutionTime = stopWatch.Elapsed.TotalMilliseconds };

      }
      catch (Exception ex)
      {
        return new MongoResponse<T>(ex);
      }
    }

    public async Task<IMongoResponse<IEnumerable<T>>> FindAsync<T>(Expression<Func<T, bool>> expression = null)
    {
      try
      {
        var collection = await Task.Run(() => this.database
          .GetCollection<T>(Helper.GetTableName(typeof(T)))
          .WithReadPreference(ReadPreference.PrimaryPreferred));

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = await collection.FindAsync(expression);
        stopWatch.Stop();

        return new MongoResponse<IEnumerable<T>>(await Task.Run(() => result.ToList())) { ExecutionTime = stopWatch.Elapsed.TotalMilliseconds };
      }
      catch (Exception ex)
      {
        return new MongoResponse<IEnumerable<T>>(ex);
      }
    }

    public async Task<IMongoResponse<IEnumerable<BsonDocument>>> Aggregate<T>(BsonDocument match, BsonDocument grouping)
    {
      var aggregateDoc = await Task.Run(() => this.database
        .GetCollection<T>(Helper.GetTableName(typeof(T)))
        .WithReadPreference(ReadPreference.PrimaryPreferred)
        .Aggregate()
        .Match(match)
        .Group(grouping));
      var stopWatch = new Stopwatch();
      stopWatch.Start();

      var result = await Task.Run(() => aggregateDoc.ToList());

      stopWatch.Stop();

      return new MongoResponse<IEnumerable<BsonDocument>>(result) { ExecutionTime = stopWatch.Elapsed.TotalMilliseconds };
    }

    private string BuildConnectionString(string host, int port, string user, string password)
    {
      return (user == string.Empty) ? $"mongodb://{host}:{port}"
                                    : $"mongodb://{user}:{password}@{host}:{port}";
    }
  }
}
