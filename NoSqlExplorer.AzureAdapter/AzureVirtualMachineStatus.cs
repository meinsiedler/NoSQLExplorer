using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Management.Compute.Models;

namespace NoSqlExplorer.AzureAdapter
{
  public enum AzureVirtualMachineStatus
  {
    Running = 0,
    Suspended = 1,
    RunningTransitioning = 2,
    SuspendedTransitioning = 3,
    Starting = 4,
    Suspending = 5,
    Deploying = 6,
    Deleting = 7
  }
}
