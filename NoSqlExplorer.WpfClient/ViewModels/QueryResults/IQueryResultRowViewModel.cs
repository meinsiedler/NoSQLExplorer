using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public interface IQueryResultRowViewModel
  {
    double DurationMillis { get; set; }
  }
}
