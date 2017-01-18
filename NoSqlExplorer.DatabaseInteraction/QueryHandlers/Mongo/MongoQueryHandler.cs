using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;
using NoSqlExplorer.Mongo.DAL.Response;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Mongo
{
  internal abstract class MongoQueryHandler<TQuery, TResult>
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
  {
    protected MongoDbClient MongoDbClient { get; }

    protected MongoQueryHandler(MongoDbClient mongoDbClient)
    {
      MongoDbClient = mongoDbClient;
    }

    public abstract Task<TResult> HandleAsync(TQuery query);

    protected Task<IMongoResponse<IEnumerable<T>>> GetResponse<T>(Expression<Func<T, bool>> expression)
    {
      return Retry.TryAwait<IMongoResponse<IEnumerable<T>>, HttpRequestException>(() => MongoDbClient.FindAsync(expression));
    }

    protected IList<T> GetResultOrThrow<T>(IMongoResponse<IEnumerable<T>> response)
    {
      if (!response.Success)
      {
        throw new DatabaseException(string.Join(Environment.NewLine, response.Errors));
      }
      return response.Data.ToList();
    }  
  }
}
