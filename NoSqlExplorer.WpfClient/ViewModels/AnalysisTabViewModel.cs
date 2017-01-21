using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.WpfClient.Messages;
using NoSqlExplorer.WpfClient.ViewModels.Queries;
using NoSqlExplorer.WpfClient.ViewModels.QueryResults;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class AnalysisTabViewModel : ViewModelBase
  {
    public AnalysisTabViewModel()
    {
      RegisterMessages();
      DefineCommands();
      InitializeQueryViewModels();
      InitializeQueryResultViewModels();
    }

    private void RegisterMessages()
    {
      Messenger.Default.Register<DatabaseInteractorsMessage>(this,
        m =>
        {
          DatabaseInteractors = new ObservableCollection<IDatabaseInteractor>(m.DatabaseInteractors);
          UpdateQueryResultViewModels();
        });
    }

    private void DefineCommands()
    {
      StartQueryCommand = new AsyncCommand(StartQueryCommandHandler,
        () => SelecteDatabaseInteractor != null && SelectedQueryViewModel != null && SelectedQueryViewModel.IsValid);
    }

    private void InitializeQueryViewModels()
    {
      QueryViewModels = new ObservableCollection<IQueryViewModel>
      {
        new GetTweetsWithHashtagQueryViewModel(),
        new GetAverageFollowersQueryViewModel()
      };
    }

    private void InitializeQueryResultViewModels()
    {
      QueryResultViewModels = new ObservableCollection<QueryResultViewModel>(QueryViewModels.Select(q => new QueryResultViewModel(q)));
    }

    private void UpdateQueryResultViewModels()
    {
      foreach (var queryResultViewModel in QueryResultViewModels)
      {
        queryResultViewModel.UpdateDatabaseResultViewModels(DatabaseInteractors);
      }
    }

    public AsyncCommand StartQueryCommand { get; private set; }

    private IDatabaseInteractor _selectedDatabaseInteractor;
    public IDatabaseInteractor SelecteDatabaseInteractor
    {
      get { return _selectedDatabaseInteractor; }
      set { Set(ref _selectedDatabaseInteractor, value); }
    }

    private ObservableCollection<IDatabaseInteractor> _databaseInteractors;
    public ObservableCollection<IDatabaseInteractor> DatabaseInteractors
    {
      get { return _databaseInteractors; }
      set { Set(ref _databaseInteractors, value); }
    } 

    private IQueryViewModel _selectedQueryViewModel;
    public IQueryViewModel SelectedQueryViewModel
    {
      get { return _selectedQueryViewModel; }
      set { Set(ref _selectedQueryViewModel, value); }
    }

    private ObservableCollection<IQueryViewModel> _queryViewModels;
    public ObservableCollection<IQueryViewModel> QueryViewModels
    {
      get { return _queryViewModels; }
      set { Set(ref _queryViewModels, value); }
    }

    private ObservableCollection<QueryResultViewModel> _queryResultViewModels;
    public ObservableCollection<QueryResultViewModel> QueryResultViewModels
    {
      get { return _queryResultViewModels; }
      set { Set(ref _queryResultViewModels, value); }
    }

    private async Task StartQueryCommandHandler()
    {
      await GetQueryResultAsync();
    }

    private async Task GetQueryResultAsync()
    {
      var getTweetsWithHashtagQueryViewModel = SelectedQueryViewModel as GetTweetsWithHashtagQueryViewModel;
      if (getTweetsWithHashtagQueryViewModel != null)
      {
        var result = await SelecteDatabaseInteractor.GetQueryResultAsync(getTweetsWithHashtagQueryViewModel.Query);
        var queryResultViewModel = QueryResultViewModels.SingleOrDefault(vm => vm.QueryViewModel == getTweetsWithHashtagQueryViewModel);
        queryResultViewModel?.PrependResult(SelecteDatabaseInteractor.ContainerName, new GetTweetsWithHashtagQueryResultViewModel(getTweetsWithHashtagQueryViewModel.Hashtag, result));
      }

      var getAverageFollowersViewModel = SelectedQueryViewModel as GetAverageFollowersQueryViewModel;
      if (getAverageFollowersViewModel != null)
      {
        var result = await SelecteDatabaseInteractor.GetQueryResultAsync(getAverageFollowersViewModel.Query);
        var queryResultViewModel = QueryResultViewModels.SingleOrDefault(vm => vm.QueryViewModel == getAverageFollowersViewModel);
        queryResultViewModel?.PrependResult(SelecteDatabaseInteractor.ContainerName, new GetAverageFollowersQueryResultViewModel(getAverageFollowersViewModel.Hashtag, result));
      }
    }
  }
}
