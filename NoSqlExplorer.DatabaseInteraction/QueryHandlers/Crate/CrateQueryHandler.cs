using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers.Crate
{
  internal abstract class CrateQueryHandler<TQuery, TResult>
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
  {
    protected CrateClient CrateClient { get; }

    protected CrateQueryHandler(CrateClient crateClient)
    {
      CrateClient = crateClient;
    }

    public abstract Task<QueryResult<TResult>> HandleAsync(TQuery query);

    protected Task<ICrateResponse<T>> GetResponse<T>(string query)
    {
      return Retry.TryAwait<ICrateResponse<T>, HttpRequestException>(() => CrateClient.SubmitQuery<T>(query));
    }

    protected QueryResult<IList<T>> GetResultOrThrow<T>(ICrateResponse<T> response)
    {
      var errorResponse = response as ErrorResponse<T>;
      if (errorResponse != null)
      {
        throw new DatabaseException(errorResponse.Error.Message);
      }

      var successResponse = response as SuccessResponse<T>;
      if (successResponse != null)
      {
        return new QueryResult<IList<T>>(successResponse.Result, successResponse.Duration);
      }

      throw new InvalidOperationException("Response is neither ErrorResponse nor SuccessResponse");
    }
  }
}
