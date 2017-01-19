using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using NoSqlExplorer.AzureAdapter;
using NoSqlExplorer.AzureAdapter.Configuration;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.DockerAdapter;
using NoSqlExplorer.DockerAdapter.Configuration;
using NoSqlExplorer.Twitter.Common;
using NoSqlExplorer.TwitterReader;
using NoSqlExplorer.TwitterReader.Configuration;
using NoSqlExplorer.TwitterReader.Model;
using NoSqlExplorer.WpfClient.Messages;
using SnackbarMessage = NoSqlExplorer.WpfClient.Messages.SnackbarMessage;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class MainWindowViewModel : ViewModelBase
  {
    private TwitterSettingsConfigElement _twitterSettingsConfigElement;
    private DockerConfigSection _dockerConfigSection;

    private readonly ITwitterReader _twitterReader;
    private readonly IList<IDatabaseInteractor> _databaseInteractors = new List<IDatabaseInteractor>(); 

    public MainWindowViewModel()
    {
      var cfg = ConfigurationManager.GetSection(TwitterConfigSection.SectionName) as TwitterConfigSection;
      _twitterSettingsConfigElement = cfg.TwitterSettings;
      _twitterReader = new TwitterReader.TwitterReader(cfg.TwitterSettings.ConsumerKey, cfg.TwitterSettings.ConsumerSecret, cfg.TwitterSettings.FeedUrl);
      _twitterReader.OnNewTweet += OnNewTweetHandler;
      RegisterMessages();
      LoadDockerInstances();
      DefineCommands();
    }

    private async void LoadDockerInstances()
    {
      _dockerConfigSection = ConfigurationManager.GetSection(DockerConfigSection.SectionName) as DockerConfigSection;
      var azureCfg = ConfigurationManager.GetSection(AzureConfigSection.SectionName) as AzureConfigSection;
      var azureController = new AzureController(azureCfg.AzureSubscription.SubscriptionId, azureCfg.AzureSubscription.Base64encodedCertificate);
      var dockerInstanceViewModels = _dockerConfigSection.DockerInstances.Select((i, idx) => new DockerInstanceViewModel(
        new DockerInstance(i.Host, i.Port, i.Username, i.Password), 
        azureController.GetVirtualMachineByHostnameAsync(azureCfg.AzureSubscription.ResourceGroup, i.Host),
        idx + 1));
      DockerInstanceViewModels = new ObservableCollection<DockerInstanceViewModel>(dockerInstanceViewModels);
      var initializeTasks = DockerInstanceViewModels.Select(i => i.InitializeAsync());
      await Task.WhenAll(initializeTasks);
    }

    private void DefineCommands()
    {
      OpenGitHubLinkCommand = new RelayCommand(() => Process.Start("https://github.com/meinsiedler/NoSQLExplorer"));

      StartAllVmsCommand = new AsyncCommand(() =>
      {
        var startTasks = DockerInstanceViewModels.Where(i => !i.IsDisabled).Select(i => i.StartVmCommandHandler());
        return Task.WhenAll(startTasks);
      });
      StopAllVmsCommand = new AsyncCommand(() =>
      {
        var stopTasks = DockerInstanceViewModels.Where(i => !i.IsDisabled).Select(i => i.StopVmCommandHandler());
        return Task.WhenAll(stopTasks);
      });
      RefreshAllVmStatusCommand = new AsyncCommand(() =>
      {
        var refreshTasks = DockerInstanceViewModels.Select(i => i.RefreshVmStatusCommandHandler());
        return Task.WhenAll(refreshTasks);
      });

      StartAllContainersCommand = new AsyncCommand(() =>
      {
        var startTasks = DockerInstanceViewModels.Where(i => !i.IsDisabled)
          .SelectMany(i => i.DockerContainerViewModels)
          .Where(c => c.ContainerName == SelectedContainerName)
          .Select(c => c.StartAsyncCommandHandler());

        return Task.WhenAll(startTasks);
      },
      () => !string.IsNullOrEmpty(SelectedContainerName));

      StopAllContainersCommand = new AsyncCommand(() =>
      {
        var stopTasks = DockerInstanceViewModels.Where(i => !i.IsDisabled)
          .SelectMany(i => i.DockerContainerViewModels)
          .Where(c => c.ContainerName == SelectedContainerName)
          .Select(c => c.StopAsyncCommandHandler());

        return Task.WhenAll(stopTasks);
      },
      () => !string.IsNullOrEmpty(SelectedContainerName));
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

      Messenger.Default.Register<ReportContainersMessage>(this, ReportContainersMessageHandler);
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

    public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();

    public RelayCommand OpenGitHubLinkCommand { get; private set; }

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
    public ICommand StartFeedReadingCommand => _startFeedReadingCommand ?? (_startFeedReadingCommand = new RelayCommand(async () => await StartReadingFeedCommandHandler(), () => !string.IsNullOrEmpty(Pin) && !IsFeedReadingRunning));

    private ICommand _stopFeedReadingCommand;
    public ICommand StopFeedReadingCommand => _stopFeedReadingCommand ?? (_stopFeedReadingCommand = new RelayCommand(StopReadingFeed, () => IsFeedReadingRunning));

    private async void GetPin()
    {
      try
      {
        Tokens = await TwitterRequest.GetRequestToken(_twitterSettingsConfigElement.ConsumerKey, _twitterSettingsConfigElement.ConsumerSecret);
        Process.Start(new ProcessStartInfo(TwitterRequest.GetRequestUrl(Tokens)));
      }
      catch (WebException ex) when (ex.Message.Contains("401"))
      {
        MessageQueue.Enqueue("Got an 401 unauthorized error. Did you set up the Twitter consumer key and consumer secret correctly?", "DISMISS", () => { });
      }
    }

    private async Task StartReadingFeedCommandHandler()
    {
      var success = await CreateDatabaseInteractors();
      if (success)
      {
        await StartReadingFeed();
      }
    }

    private async Task<bool> CreateDatabaseInteractors()
    {
      _databaseInteractors.Clear();

      foreach (var containerConfig in _dockerConfigSection.DockerContainer)
      {
        var containerWithName = DockerInstanceViewModels
          .SelectMany(i => i.DockerContainerViewModels)
          .Where(c => c.ContainerName == containerConfig.Name)
          .ToList();

        if (containerWithName.All(c => c.ContainerState != DockerContainerState.Started))
        {
          MessageQueue.Enqueue($"Inserting cannot be started since no container with name '{containerConfig.Name}' is started.", "DISMISS", () => { });
          return false;
        }

        var host = containerWithName.First(c => c.ContainerState == DockerContainerState.Started).Host;
        IDatabaseInteractor databaseInteractor = DatabaseInteractorFactory.CreateDatabaseInteractor(
          containerConfig.Name,
          host,
          _dockerConfigSection);

        if (databaseInteractor == null)
        {
          MessageQueue.Enqueue($"No Twitter loader available for container '{containerConfig.Name}'.", "DISMISS", () => { });
          return false;
        }

        await databaseInteractor.EnsureTableExistsAsync();
        _databaseInteractors.Add(databaseInteractor);
      }

      return true;
    }

    private async Task StartReadingFeed()
    {
      try
      {
        var tokens = await TwitterRequest.GetAccessToken(_twitterSettingsConfigElement.ConsumerKey, _twitterSettingsConfigElement.ConsumerSecret , Tokens.OAuthToken, Tokens.OAuthSecret, Pin);
        Pin = string.Empty;
        if (tokens == null) return;

        if (!string.IsNullOrEmpty(tokens.UserId))
        {
          IsFeedReadingRunning = true;
          MessageQueue.Enqueue("Loading of Twitter messages started.");
          await _twitterReader.StartAsync(tokens.OAuthToken, tokens.OAuthSecret);
        }
        else
        {
          MessageQueue.Enqueue("You are not propertly authenticated with your Twitter account.", "DISMISS", () => { });
        }
      }
      catch (WebException ex) when (ex.Message.Contains("401"))
      {
        MessageQueue.Enqueue("Got an 401 unauthorized error. Are you properly authenticated and did you use a new PIN?", "DISMISS", () => { });
      }
      catch (Exception ex)
      {
        MessageQueue.Enqueue("An error occured. Are you using a correct PIN?", "DISMISS", () => { });
      }
    }

    private void StopReadingFeed()
    {
      _twitterReader.Stop();
      IsFeedReadingRunning = false;
      MessageQueue.Enqueue("Loading of Twitter messages stopped.");
    }
    
    private async void OnNewTweetHandler(Tweet tweet)
    {
      Dispatcher.CurrentDispatcher.Invoke(() => FeedsCount++);
      foreach (var databaseInteractor in _databaseInteractors)
      {
        try
        {
          await databaseInteractor.BulkInsertAsync(new[] {tweet});
        }
        catch (HttpRequestException)
        {
          if (IsFeedReadingRunning)
          {
            StopReadingFeed();
            MessageQueue.Enqueue(
              $"Inserting stopped, because container '{databaseInteractor.ContainerName}' on host {databaseInteractor.Host} is not reachable.",
              "DISMISS", () => { });
          }
        }
      }
    }

    private ObservableCollection<DockerInstanceViewModel> _dockerInstanceViewModels;
    
    public ObservableCollection<DockerInstanceViewModel> DockerInstanceViewModels
    {
      get { return _dockerInstanceViewModels; }
      set { Set(ref _dockerInstanceViewModels, value); }
    }

    public AsyncCommand StartAllVmsCommand { get; private set; }
    public AsyncCommand StopAllVmsCommand { get; private set; }
    public AsyncCommand RefreshAllVmStatusCommand { get; private set; }

    public AsyncCommand StartAllContainersCommand { get; private set; }
    public AsyncCommand StopAllContainersCommand { get; private set; }

    private readonly IDictionary<int, IList<string>> _containers = new Dictionary<int, IList<string>>();

    private ObservableCollection<string> _containerNames = new ObservableCollection<string>();
    public ObservableCollection<string> ContainerNames
    {
      get { return _containerNames; }
      set { Set(ref _containerNames, value); }
    }

    private string _selectedContainerName;
    public string SelectedContainerName
    {
      get { return _selectedContainerName; }
      set { Set(ref _selectedContainerName, value); }
    }

    private void ReportContainersMessageHandler(ReportContainersMessage message)
    {
      _containers[message.DockerInstanceNumber] = message.ContainerNames.ToList();
      UpdateContainerNames();
    }

    private void UpdateContainerNames()
    {
      var containerNames = _containers.SelectMany(c => c.Value).Distinct().OrderBy(n => n);
      ContainerNames = new ObservableCollection<string>(containerNames);
    }
  }
}
