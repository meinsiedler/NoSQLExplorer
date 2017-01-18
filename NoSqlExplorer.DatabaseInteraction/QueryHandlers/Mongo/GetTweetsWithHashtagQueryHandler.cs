using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Mongo.DAL;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Mongo
{
  internal class GetTweetsWithHashtagQueryHandler : MongoQueryHandler<GetTweetsWithHashtagQuery, IList<Tweet>>
  {
    public GetTweetsWithHashtagQueryHandler(MongoDbClient mongoDbClient) : base(mongoDbClient)
    {
    }

    public override Task<IList<Tweet>> HandleAsync(GetTweetsWithHashtagQuery query)
    {
      throw new NotImplementedException();
    }
  }
}
