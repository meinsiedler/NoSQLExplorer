using System.Collections.Generic;
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

    public override async Task<IList<Tweet>> HandleAsync(GetTweetsWithHashtagQuery query)
    {
      var response = await GetResponse<Tweet>(t => t.Text.Contains(query.Hashtag));
      return GetResultOrThrow(response);
    }
  }
}
