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

    public override Task<double> HandleAsync(GetAverageFollowersQuery query)
    {
      throw new NotImplementedException();
    }
  }
}
