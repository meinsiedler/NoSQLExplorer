using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.WpfClient.ViewModels.Queries;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public class QueryResultViewModel : ViewModelBase
  {
    public QueryResultViewModel(IQueryViewModel queryViewModelViewModel, IList<IDatabaseInteractor> databaseInteractors)
    {
      QueryViewModel = queryViewModelViewModel;
      QueryName = queryViewModelViewModel.QueryName;
      DatabaseResultViewModels = new ObservableCollection<DatabaseResultViewModel>(databaseInteractors.Select(di => new DatabaseResultViewModel(di.ContainerName)));
    }

    public IQueryViewModel QueryViewModel { get; }
    public string QueryName { get; }

    private ObservableCollection<DatabaseResultViewModel> _databaseResultViewModels;
    public ObservableCollection<DatabaseResultViewModel> DatabaseResultViewModels
    {
      get { return _databaseResultViewModels;}
      set { Set(ref _databaseResultViewModels, value); }
    }
     
    public void PrependResult(string containerName, IQueryResultRowViewModel queryResultRowViewModel)
    {
      DatabaseResultViewModels.Single(vm => vm.ContainerName == containerName).QueryResultRows.Insert(0, queryResultRowViewModel);
    }
  }
}
