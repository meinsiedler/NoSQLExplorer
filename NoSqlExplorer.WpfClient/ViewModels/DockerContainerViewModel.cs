﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NoSqlExplorer.DockerAdapter;
using NoSqlExplorer.WpfClient.Messages;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class DockerContainerViewModel : ViewModelBase
  {
    public string Host { get; }
    private readonly DockerContainer _container;

    public DockerContainerViewModel(DockerContainer container, DockerInstance instance)
    {
      _container = container;
      _containerName = container.Name;
      _containerState = container.State;
      Host = instance.Host;
      StartAsyncCommand = new AsyncCommand(StartAsyncCommandHandler);
      StopAsyncCommand = new AsyncCommand(StopAsyncCommandHandler);
    }

    private string _containerName;
    public string ContainerName
    {
      get { return _containerName; }
      set { Set(ref _containerName, value); }
    }

    private DockerContainerState _containerState;
    public DockerContainerState ContainerState
    {
      get { return _containerState; }
      set { Set(ref _containerState, value); }
    }

    public AsyncCommand StartAsyncCommand { get; private set; }
    public AsyncCommand StopAsyncCommand { get; private set; }

    private bool _isBusy;
    public bool IsBusy
    {
      get { return _isBusy; }
      set { Set(ref _isBusy, value); }
    }

    public async Task StartAsyncCommandHandler()
    {
      IsBusy = true;
      await _container.StartAsync();
      UpdateState();
      IsBusy = false;
    }

    public async Task StopAsyncCommandHandler()
    {
      IsBusy = true;
      await _container.StopAsync();
      UpdateState();
      IsBusy = false;
    }

    private void UpdateState()
    {
      this.ContainerState = _container.State;
      Messenger.Default.Send(new ReevaluateDatabaseInteractorsMessage());
    }
  }
}
