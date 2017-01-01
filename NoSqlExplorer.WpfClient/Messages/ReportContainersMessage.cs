using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.WpfClient.Messages
{
  public class ReportContainersMessage
  {
    public ReportContainersMessage(int dockerInstanceNumber, IEnumerable<string> containerNames)
    {
      DockerInstanceNumber = dockerInstanceNumber;
      ContainerNames = containerNames;
    }

    public int DockerInstanceNumber { get; }
    public IEnumerable<string> ContainerNames { get; }
  }
}
