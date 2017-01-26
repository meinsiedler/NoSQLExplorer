using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using NoSqlExplorer.Utils;

namespace NoSqlExplorer.DockerAdapter
{
  public class DockerInstance
  {
    private DockerClient client;

    public string Host { get; }
    public int Port { get; }

    public DockerInstance(string host, int port, string userName, string password)
    {
      Host = host;
      Port = port;
      var credentials = new BasicAuthCredentials(userName, password);
      this.client = new DockerClientConfiguration(new Uri($"http://{host}:{port}"), credentials).CreateClient();
      
    }

    public async Task<IEnumerable<DockerContainer>> GetContainersAsync()
    {
      // The default timeout is 100 seconds for the HTTP request. The Docker client doesn't allow to configure another timeout.
      // Therefore, we just try to get the containers a few times.
      var containers = await Retry.TryAwait<IList<ContainerListResponse>, Exception>(() => client.Containers.ListContainersAsync(new ContainersListParameters
      {
        All = true
      }));

      return containers.Select(c => new DockerContainer(this.client)
      {
        Id = c.ID,
        Name = c.Names.FirstOrDefault(),
        State = c.State == "exited" ? DockerContainerState.Exited : DockerContainerState.Started,
        Status = c.Status
      });
    }

    
  }
}
