using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;

namespace NoSqlExplorer.DatabaseInteraction.QueryHandlers
{
  internal interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}
