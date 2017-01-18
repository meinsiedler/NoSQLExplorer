using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Crate
{
  internal class GetAverageFollowersQueryHandler : CrateQueryHandler<GetAverageFollowersQuery, double>
  {
    public GetAverageFollowersQueryHandler(CrateClient crateClient) : base(crateClient)
    {
    }

    private string BuildQuery()
    {
      return "SELECT AVG(Followers) FROM Tweets";
    }

    public override async Task<double> HandleAsync(GetAverageFollowersQuery query)
    {
      var response = await Retry.TryAwait<ICrateResponse<double>, HttpRequestException>(() => CrateClient.SubmitQuery<double>(BuildQuery()));
      var result = GetResultOrThrow(response);

      if (result.Count != 1)
      {
        throw new DatabaseException($"Invalid result for {nameof(GetAverageFollowersQuery)}");
      }
      return result[0];
    }
  }
}
