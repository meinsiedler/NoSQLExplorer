using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public class DatabaseResultViewModel : ViewModelBase
  {
    public DatabaseResultViewModel(string containerName)
    {
      ContainerName = containerName;
      QueryResultRows = new ObservableCollection<IQueryResultRowViewModel>();
      QueryResultRows.CollectionChanged += QueryResultRowsOnCollectionChanged;
    }

    public string ContainerName { get; }

    public int Runs => QueryResultRows.Count;
    public double AverageDuration => QueryResultRows.Any() ? QueryResultRows.Average(r => r.DurationMillis) : 0;

    private ICommand _clearResultsCommand;
    public ICommand ClearResultsCommand => _clearResultsCommand ?? (_clearResultsCommand = new RelayCommand(() => QueryResultRows.Clear()));

    private ObservableCollection<IQueryResultRowViewModel> _queryResultRows;

    public ObservableCollection<IQueryResultRowViewModel> QueryResultRows
    {
      get { return _queryResultRows; }
      set { Set(ref _queryResultRows, value); }
    }

    private void QueryResultRowsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
      RaisePropertyChanged(() => Runs);
      RaisePropertyChanged(() => AverageDuration);
    }
  }
}
