using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;

namespace NoSqlExplorer.DockerAdapter
{
  public class DockerInstance
  {
    private DockerClient client;

    public DockerInstance(string host, int port, string userName, string password)
    {
      var credentials = new BasicAuthCredentials(userName, password);
      this.client = new DockerClientConfiguration(new Uri($"http://{host}:{port}"), credentials).CreateClient();
    }

    public async Task<IEnumerable<DockerContainer>> GetContainers()
    {
      var containers = await client.Containers.ListContainersAsync(new ContainersListParameters
      {
        All = true
      });

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
