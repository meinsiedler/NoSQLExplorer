using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DatabaseInteraction
{
  public class QueryResult<TResult>
  {
    public QueryResult(TResult result, double? durationMillis)
    {
      Result = result;
      DurationMillis = durationMillis;
    }

    public TResult Result { get; }
    public double? DurationMillis { get; }
  }
}
