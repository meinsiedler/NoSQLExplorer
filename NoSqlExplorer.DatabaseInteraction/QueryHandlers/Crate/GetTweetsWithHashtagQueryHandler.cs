using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Twitter.Common;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Crate
{
  internal class GetTweetsWithHashtagQueryHandler
    : CrateQueryHandler<GetTweetsWithHashtagQuery, IList<Tweet>>
  {
    public GetTweetsWithHashtagQueryHandler(CrateClient crateClient) : base(crateClient)
    {
    }

    private string BuildQuery(GetTweetsWithHashtagQuery query)
    {
      // TODO: inject Tweets table name from outside?
      // TODO: Note that LIKE can be pretty slow -> consider fulltext index, see https://crate.io/docs/reference/sql/queries.html#like
      // TODO: escape hashtag string?
      return $"SELECT * FROM Tweets where Text LIKE '%{query.Hashtag}%'";
    }

    public override async Task<QueryResult<IList<Tweet>>> HandleAsync(GetTweetsWithHashtagQuery query)
    {
      var response = await GetResponse<Tweet>(BuildQuery(query));
      return GetResultOrThrow(response);
    }
  }
}
