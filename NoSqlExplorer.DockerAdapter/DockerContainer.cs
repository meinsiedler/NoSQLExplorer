using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace NoSqlExplorer.DockerAdapter
{
  public class DockerContainer
  {
    private DockerClient client;

    internal DockerContainer(DockerClient client)
    {
      this.client = client;
    }

    public string Id { get; internal set; }
    public string Name { get; internal set; }
    public DockerContainerState State { get; internal set; }
    public string Status { get; internal set; }

    public async Task<bool> StartAsync()
    {
      var previousState = this.State;
      var started = await this.client.Containers.StartContainerAsync(this.Id, new ContainerStartParameters());
      this.State = started || previousState == DockerContainerState.Started ? DockerContainerState.Started : DockerContainerState.Exited;
      return started;
    }

    public async Task<bool> StopAsync()
    {
      var previousState = this.State;
      var stopped = await this.client.Containers.StopContainerAsync(this.Id, new ContainerStopParameters(), System.Threading.CancellationToken.None);
      this.State = stopped || previousState == DockerContainerState.Exited ? DockerContainerState.Exited : DockerContainerState.Started;
      return stopped;
    }

    public async Task<Stream> GetContainerLogsAsync(System.Threading.CancellationToken ct)
    {
      return await this.client.Containers.GetContainerLogsAsync(
        this.Id,
        new ContainerLogsParameters()
        {
          ShowStderr = true,
          ShowStdout = true,
        },
        ct);
    }
  }
}
