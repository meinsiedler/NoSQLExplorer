using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL.Response
{
  public interface ICrateResponse
  {
  }

  public interface ICrateResponse<T> : ICrateResponse where T : class
  {
  }
}
