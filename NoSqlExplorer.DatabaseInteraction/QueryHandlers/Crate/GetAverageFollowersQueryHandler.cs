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
    internal class ResultWrapper
    {
      public double Followers { get; set; }
    }

    public GetAverageFollowersQueryHandler(CrateClient crateClient) : base(crateClient)
    {
    }

    private string BuildQuery()
    {
      return "SELECT AVG(Followers) AS Followers FROM Tweets";
    }

    public override async Task<double> HandleAsync(GetAverageFollowersQuery query)
    {
      var response = await Retry.TryAwait<ICrateResponse<ResultWrapper>, HttpRequestException>(() => CrateClient.SubmitQuery<ResultWrapper>(BuildQuery()));
      var result = GetResultOrThrow(response);

      if (result.Count != 1)
      {
        throw new DatabaseException($"Invalid result for {nameof(GetAverageFollowersQuery)}");
      }
      return result[0].Followers;
    }
  }
}
