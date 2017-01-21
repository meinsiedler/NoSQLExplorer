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
    public QueryResultViewModel(IQueryViewModel queryViewModelViewModel)
    {
      QueryViewModel = queryViewModelViewModel;
      QueryName = queryViewModelViewModel.QueryName;
      DatabaseResultViewModels = new ObservableCollection<DatabaseResultViewModel>();
    }

    public IQueryViewModel QueryViewModel { get; }
    public string QueryName { get; }

    public void UpdateDatabaseResultViewModels(IList<IDatabaseInteractor> databaseInteractors)
    {
      var newDatabaseInteractors = databaseInteractors.Where(di => DatabaseResultViewModels.All(vm => vm.ContainerName != di.ContainerName));
      foreach (var newDatabaseInteractor in newDatabaseInteractors)
      {
        DatabaseResultViewModels.Add(new DatabaseResultViewModel(newDatabaseInteractor.ContainerName));
      }
      DatabaseResultViewModels = new ObservableCollection<DatabaseResultViewModel>(DatabaseResultViewModels.OrderBy(vm => vm.ContainerName));
    }

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
