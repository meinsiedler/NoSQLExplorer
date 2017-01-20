using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using SnackbarMessage = NoSqlExplorer.WpfClient.Messages.SnackbarMessage;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      RegisterMessages();

      OpenGitHubLinkCommand = new RelayCommand(() => Process.Start("https://github.com/meinsiedler/NoSQLExplorer"));
      StartTabViewModel = new StartTabViewModel();
      AnalysisTabViewModel = new AnalysisTabViewModel();
    }

    private void RegisterMessages()
    {
      Messenger.Default.Register<SnackbarMessage>(this, m =>
      {
        if (string.IsNullOrEmpty(m.Action))
        {
          MessageQueue.Enqueue(m.Text);
        }
        else
        {
          MessageQueue.Enqueue(m.Text, m.Action, () => { });
        }
      });
    }

    public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();

    public RelayCommand OpenGitHubLinkCommand { get; private set; }

    private StartTabViewModel _startTabViewModel;
    public StartTabViewModel StartTabViewModel
    {
      get { return _startTabViewModel; }
      set { Set(ref _startTabViewModel, value); }
    }

    private AnalysisTabViewModel _analysisTabViewModel;
    public AnalysisTabViewModel AnalysisTabViewModel
    {
      get { return _analysisTabViewModel; }
      set { Set(ref _analysisTabViewModel, value); }
    }
  }
}
