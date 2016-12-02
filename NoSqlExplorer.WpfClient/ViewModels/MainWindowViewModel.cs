using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using NoSqlExplorer.TwitterReader;
using NoSqlExplorer.TwitterReader.Model;
using NoSqlExplorer.WpfClient.Messages;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    private readonly ITwitterReader _twitterReader;

    public MainWindowViewModel()
    {
      _twitterReader = new TwitterReader.TwitterReader("https://stream.twitter.com/1.1/statuses/sample.json");
      _twitterReader.OnNewTweet += tweet => Dispatcher.CurrentDispatcher.Invoke(() => FeedsCount++);
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

    private string _pin;
    public string Pin
    {
      get { return _pin; }
      set { Set(ref _pin, value); }
    }

    private string _webAddress;
    public string WebAddress
    {
      get { return _webAddress; }
      set { Set(ref _webAddress, value); }
    }

    private OAuthTokens _tokens;
    private OAuthTokens Tokens
    {
      get { return _tokens; }
      set { Set(ref _tokens, value); }
    }

    private string _accessToken;
    public string AccessToken
    {
      get { return _accessToken; }
      set { Set(ref _accessToken, value); }
    }

    private bool _isFeedReadingRunning;
    public bool IsFeedReadingRunning
    {
      get { return _isFeedReadingRunning; }
      set { Set(ref _isFeedReadingRunning, value); }
    }

    private int _feedsCount;
    public int FeedsCount
    {
      get { return _feedsCount; }
      set { Set(ref _feedsCount, value); }
    }

    private ICommand _getPinCommand;
    public ICommand GetPinCommand => _getPinCommand ?? (_getPinCommand = new RelayCommand(GetPin));

    private ICommand _startFeedReadingCommand;
    public ICommand StartFeedReadingCommand => _startFeedReadingCommand ?? (_startFeedReadingCommand = new RelayCommand(async () => await StartReadingFeed(), () => !string.IsNullOrEmpty(Pin) && !IsFeedReadingRunning));

    private ICommand _stopFeedReadingCommand;
    public ICommand StopFeedReadingCommand => _stopFeedReadingCommand ?? (_stopFeedReadingCommand = new RelayCommand(StopReadingFeed, () => IsFeedReadingRunning));

    private async void GetPin()
    {
      try
      {
        Tokens = await Twitter.GetRequestToken();
        var url = "https://api.twitter.com/oauth/authenticate?oauth_token=" + Tokens.OAuthToken;

        Process.Start(new ProcessStartInfo(url));
      }
      catch (AggregateException ex)
      {
        Trace.TraceError(ex.Message, ex);
      }
    }

    private async Task StartReadingFeed()
    {
      try
      {
        var tokens = await Twitter.GetAccessToken(Tokens.OAuthToken, Tokens.OAuthSecret, Pin);
        Pin = string.Empty;
        if (tokens == null) return;

        if (!string.IsNullOrEmpty(tokens.UserId))
        {
          IsFeedReadingRunning = true;
          await _twitterReader.StartAsync(tokens.OAuthToken, tokens.OAuthSecret);
        }
        else
        {
          MessageBox.Show("Not authenticated ...");
        }
      }
      catch (WebException ex) when (ex.Message.Contains("401"))
      {
        MessageBox.Show("Got an 401 unauthorized error. Are you properly authenticated and did you use a new PIN?", "Error",
          MessageBoxButton.OK, MessageBoxImage.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"An exception occured. Details: {Environment.NewLine}{ex.Message}", "Error",
          MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void StopReadingFeed()
    {
      _twitterReader.Stop();
      IsFeedReadingRunning = false;
    }
  }
}
