using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public class QueryResultViewModel : ViewModelBase
  {
    public QueryResultViewModel(string queryName)
    {
      QueryName = queryName;
    }

    public string QueryName { get; }

    public DatabaseResultViewModel CrateResultViewModel { get; } = new DatabaseResultViewModel("Create.io");
    public DatabaseResultViewModel MongoResultViewModel { get; } = new DatabaseResultViewModel("MongoDB");

  }
}
