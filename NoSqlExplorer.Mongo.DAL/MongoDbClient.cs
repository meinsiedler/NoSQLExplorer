using System;
using System.Collections.Generic;
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
        await this.database.GetCollection<T>(Helper.GetTableName(typeof(T))).InsertOneAsync(item, new InsertOneOptions { BypassDocumentValidation = true });
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
        var execTime = this.GetExecutionTime(expression);

        var result = await this.database
        .GetCollection<T>(Helper.GetTableName(typeof(T)))
        .AsQueryable()
        .SingleOrDefaultAsync(expression);

        return new MongoResponse<T>(result) { ExecutionTime = execTime };

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
        var execTime = this.GetExecutionTime(expression);

        var result = await this.database
          .GetCollection<T>(Helper.GetTableName(typeof(T)))
          .FindAsync(expression);
        return new MongoResponse<IEnumerable<T>>(result.ToList()) { ExecutionTime = execTime };
      }
      catch (Exception ex)
      {
        return new MongoResponse<IEnumerable<T>>(ex);
      }
    }

    private string BuildConnectionString(string host, int port, string user, string password)
    {
      return $"mongodb://{user}:{password}@{host}:{port}";
    }

    private double? GetExecutionTime<T>(Expression<Func<T, bool>> expression)
    {
      var options = new FindOptions
      {
        Modifiers = new BsonDocument("$explain", true)
      };

      var coll = this.database.GetCollection<T>(Helper.GetTableName(typeof(T)));
      var explainResult = coll.Find(expression, options).Project(new BsonDocument()).FirstOrDefault();
      if (explainResult.ElementCount == 3)
      {
        return explainResult["executionStats"]["executionTimeMillis"].ToDouble();
      }

      return -1.0;
    }
  }
}
