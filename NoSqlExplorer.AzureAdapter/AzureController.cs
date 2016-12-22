using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;

namespace NoSqlExplorer.AzureAdapter
{
  public class AzureController : IDisposable
  {
    private ComputeManagementClient _computeManagementClient;

    public AzureController(
      string subscriptionId,
      string base64EncodedCert)
    {
      try
      {
        var credentials = GetCredentials(subscriptionId, base64EncodedCert);
        _computeManagementClient = new ComputeManagementClient(credentials);
      }
      catch (Exception)
      {
        // should only occur when certificate is invalid base64 string. 
      }
    }

    public async Task<AzureVirtualMachine> GetVirtualMachineByHostnameAsync(string resourceGroup, string hostName)
    {
      var hostedServices = await _computeManagementClient.HostedServices.ListAsync();
      var resGroupMachines = hostedServices.Where(vm => vm.Properties.ExtendedProperties["ResourceGroup"] == resourceGroup);
      foreach (var machine in resGroupMachines)
      {
        var detailedInfos = await _computeManagementClient.HostedServices.GetDetailedAsync(machine.ServiceName);
        var deploymentInfo = detailedInfos.Deployments.FirstOrDefault(d => d.Uri.Host == hostName);
        if (deploymentInfo != null)
        {
          return new AzureVirtualMachine(this, machine.ServiceName, deploymentInfo.Name, deploymentInfo.Name);
        }
      }

      return null;
    }

    internal async Task<OperationStatusResponse> StartAsync(string serviceName, string deploymentName, string vmName)
    {
      return await _computeManagementClient.VirtualMachines.StartAsync(serviceName, deploymentName, vmName);
    }

    internal async Task<OperationStatusResponse> ShutdownAsync(string serviceName, string deploymentName, string vmName)
    {
      var shutdownParams = new VirtualMachineShutdownParameters
      {
        PostShutdownAction = PostShutdownAction.StoppedDeallocated
      };

      return await _computeManagementClient.VirtualMachines.ShutdownAsync(serviceName, deploymentName, vmName, shutdownParams);
    }

    internal async Task<DeploymentStatus?> GetStatusAsync(string serviceName)
    {
      var detailedInfos = await _computeManagementClient.HostedServices.GetDetailedAsync(serviceName);
      return detailedInfos.Deployments.FirstOrDefault()?.Status;
    }

    internal static SubscriptionCloudCredentials GetCredentials(
     string subscriptionId,
     string base64EncodedCert)
    {
      return new CertificateCloudCredentials(
        subscriptionId,
        new X509Certificate2(Convert.FromBase64String(base64EncodedCert)));
    }

    public void Dispose()
    {
      _computeManagementClient.Dispose();
    }
  }
}
