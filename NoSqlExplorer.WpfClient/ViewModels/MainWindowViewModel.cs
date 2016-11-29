using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using NoSqlExplorer.WpfClient.Messages;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      RegisterMessages();
    }

    private void RegisterMessages()
    {
      Messenger.Default.Register<IsLoadingMessage>(this, m =>
      {
        IsLoading = m.IsLoading;
        IsLoadingReason = m.Reason;
      });
    }

    private bool _isLoading;
    public bool IsLoading
    {
      get { return _isLoading; }
      set { Set(ref _isLoading, value); }
    }

    private string _isLoadingReason;
    public string IsLoadingReason
    {
      get { return _isLoadingReason; }
      set { Set(ref _isLoadingReason, value); }
    }

    private ICommand _demoCommand;
    public ICommand DemoCommand => _demoCommand ?? (_demoCommand = new RelayCommand(Demo));

    private async void Demo()
    {
      Messenger.Default.Send(new IsLoadingMessage {IsLoading = true, Reason = "I'm loading ..."});
      await Task.Delay(2000);
      Messenger.Default.Send(new IsLoadingMessage {IsLoading = false});
    }
  }
}
