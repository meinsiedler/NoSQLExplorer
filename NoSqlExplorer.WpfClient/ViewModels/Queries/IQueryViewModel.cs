using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;

namespace NoSqlExplorer.WpfClient.ViewModels.Queries
{
  public interface IQueryViewModel
  {
    string QueryName { get; }
    bool IsValid { get; }
  }
}
