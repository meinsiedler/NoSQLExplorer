using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Mongo
{
  internal class GetAverageFollowersQueryHandler : MongoQueryHandler<GetAverageFollowersQuery, double>
  {
    public GetAverageFollowersQueryHandler(MongoDbClient mongoDbClient) : base(mongoDbClient)
    {
    }

    public override async Task<QueryResult<double>> HandleAsync(GetAverageFollowersQuery query)
    {
      var groupingDoc = new BsonDocument { { "_id", "null" }, { "avgFollowers", new BsonDocument("$avg", "$Followers") } };

      var filter = !string.IsNullOrEmpty(query.Hashtag)
        ? new BsonDocument {{"$text", new BsonDocument("$search", query.Hashtag)}}
        : new BsonDocument();

      var aggregateResult = await MongoDbClient.Aggregate<Tweet>(filter, groupingDoc);

      return new QueryResult<double>(aggregateResult.Data.First()["avgFollowers"].AsDouble, aggregateResult.ExecutionTime);
    }
  }
}
