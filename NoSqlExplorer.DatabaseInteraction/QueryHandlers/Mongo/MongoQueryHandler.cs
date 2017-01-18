using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;

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
  }
}
