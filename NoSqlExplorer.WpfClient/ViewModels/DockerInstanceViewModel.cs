using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NoSqlExplorer.AzureAdapter;
using NoSqlExplorer.DockerAdapter;

namespace NoSqlExplorer.WpfClient.ViewModels
{
  public class DockerInstanceViewModel : ViewModelBase
  {
    private readonly DockerInstance _dockerInstance;
    private AzureVirtualMachine _virtualMachine;
    private int _port;

    public DockerInstanceViewModel(DockerInstance dockerInstance, Task<AzureVirtualMachine> virtualMachine, int number)
    {
      _dockerInstance = dockerInstance;
      Host = _dockerInstance.Host;
      Port = _dockerInstance.Port;
      Number = number;
      this.StartVmCommand = new AsyncCommand(this.StartVmCommandHandler);
      this.StopVmCommand = new AsyncCommand(this.StopVmCommandHandler);
      this.RefreshVmStatusCommand = new AsyncCommand(this.RefreshVmStatusCommandHandler);
      this.InitializeVmAsync(virtualMachine);
    }
    private async void InitializeVmAsync(Task<AzureVirtualMachine> virtualMachine)
    {
      this.IsBusy = true;
      _virtualMachine = await virtualMachine;
      this.VmStatus = await _virtualMachine.GetStatusAsync();
      this.IsBusy = false;
    }

    private bool isBusy;

    public bool IsBusy
    {
      get { return this.isBusy; }
      set { this.Set(ref isBusy, value); }
    }

    private int _number;
    public int Number
    {
      get { return _number; }
      set { Set(ref _number, value); }
    }

    private string _host;
    public string Host
    {
      get { return _host; }
      set { Set(ref _host, value); }
    }

    public int Port
    {
      get { return _port; }
      set { Set(ref _port, value); }
    }

    private AzureVirtualMachineStatus _vmStatus = AzureVirtualMachineStatus.Unknown;
    public AzureVirtualMachineStatus VmStatus
    {
      get { return _vmStatus; }
      set { Set(ref _vmStatus, value); }
    }

    public async Task UpdateStatusAsync()
    {
      this.VmStatus = await _virtualMachine.GetStatusAsync();
    }

    public AsyncCommand StartVmCommand { get; set; }
    public AsyncCommand StopVmCommand { get; set; }
    public AsyncCommand RefreshVmStatusCommand { get; set; }

    private async Task StartVmCommandHandler()
    {
      this.IsBusy = true;
      await _virtualMachine.StartAsync();
      await this.UpdateStatusAsync();
      this.IsBusy = false;
    }

    private async Task StopVmCommandHandler()
    {
      this.IsBusy = true;
      await _virtualMachine.ShutdownAsync();
      await this.UpdateStatusAsync();
      this.IsBusy = false;
    }

    private async Task RefreshVmStatusCommandHandler()
    {
      this.IsBusy = true;
      await this.UpdateStatusAsync();
      this.IsBusy = false;
    }

  }
}
