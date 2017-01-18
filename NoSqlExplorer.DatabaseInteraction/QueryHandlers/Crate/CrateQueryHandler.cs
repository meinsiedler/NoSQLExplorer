using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL;
using NoSqlExplorer.DatabaseInteraction.Queries;

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

    public abstract Task<TResult> HandleAsync(TQuery query);
  }
}
