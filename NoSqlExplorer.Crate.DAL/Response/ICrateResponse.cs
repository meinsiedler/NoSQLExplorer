using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL.Response
{
  public interface ICrateResponse
  {
    TimeSpan? ExecutionTime { get; set; }
  }

  public interface ICrateResponse<T> : ICrateResponse
  {
  }
}
