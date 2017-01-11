using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
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
      var connectionString = string.Empty; 
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

  }
}
