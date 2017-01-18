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

    public string BuildQuery(GetTweetsWithHashtagQuery query)
    {
      // TODO: inject Tweets table name from outside?
      // TODO: Not that LIKE can be pretty slow -> consider fulltext index, see https://crate.io/docs/reference/sql/queries.html#like
      // TODO: escape hashtag string?
      return $"SELECT * FROM Tweets where Text LIKE '%{query.Hashtag}%'";
    }

    public override async Task<IList<Tweet>> HandleAsync(GetTweetsWithHashtagQuery query)
    {
      var response = await Retry.TryAwait<ICrateResponse<Tweet>, HttpRequestException>(() => CrateClient.SubmitQuery<Tweet>(BuildQuery(query)));

      var errorResponse = response as ErrorResponse<Tweet>;
      if (errorResponse != null)
      {
        throw new DatabaseException(errorResponse.Error.Message);
      }

      var successResponse = response as SuccessResponse<Tweet>;
      if (successResponse != null)
      {
        return successResponse.Result;
      }

      throw new InvalidOperationException("Response is neither ErrorResponse nor SuccessResponse");
    }
  }
}
