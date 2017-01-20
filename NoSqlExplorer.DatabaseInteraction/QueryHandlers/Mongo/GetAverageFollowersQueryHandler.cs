using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Mongo
{
  internal class GetAverageFollowersQueryHandler : MongoQueryHandler<GetAverageFollowersQuery, double>
  {
    public GetAverageFollowersQueryHandler(MongoDbClient mongoDbClient) : base(mongoDbClient)
    {
    }

    public override Task<QueryResult<double>> HandleAsync(GetAverageFollowersQuery query)
    {
      // TODO: handle optional Hashtag, if Hashtag is not provided: calculate average followers for all tweets
      throw new NotImplementedException();
    }
  }
}
