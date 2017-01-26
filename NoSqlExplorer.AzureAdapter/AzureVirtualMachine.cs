using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.AzureAdapter
{
  public class AzureVirtualMachine
  {
    private AzureController _azureController;
    private string _deploymentName;
    private string _serviceName;
    private string _vmName;

    internal AzureVirtualMachine(AzureController azureController, string serviceName, string deploymentName, string vmName)
    {
      _azureController = azureController;
      _serviceName = serviceName;
      _deploymentName = deploymentName;
      _vmName = vmName;
    }

    public async Task<AzureVirtualMachineStatus> GetStatusAsync()
    {
      var deploymentStatus = await _azureController.GetStatusAsync(_serviceName);
      if(deploymentStatus == null)
      {
        return AzureVirtualMachineStatus.Unknown;
      }

      return (AzureVirtualMachineStatus)((int)deploymentStatus);
    }

    public async Task<CommandResponse> StartAsync()
    {
      var result = await _azureController.StartAsync(_serviceName, _deploymentName, _vmName);
      if (result.Status == Microsoft.Azure.OperationStatus.Succeeded)
      {
        return new CommandResponse(true);
      }
      else
      {
        return new CommandResponse(false, result.Error.Message);
      }
    }

    public async Task<CommandResponse> ShutdownAsync()
    {
      var result = await _azureController.ShutdownAsync(_serviceName, _deploymentName, _vmName);
      if (result.Status == Microsoft.Azure.OperationStatus.Succeeded)
      {
        return new CommandResponse(true);
      }
      else
      {
        return new CommandResponse(false, result.Error.Message);
      }
    }
  }
}
