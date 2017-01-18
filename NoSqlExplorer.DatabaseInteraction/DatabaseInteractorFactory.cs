using System.Linq;
using NoSqlExplorer.DockerAdapter.Configuration;

namespace NoSqlExplorer.DatabaseInteraction
{
  public static class DatabaseInteractorFactory
  {
    public static IDatabaseInteractor CreateDatabaseInteractor(string name, string host, DockerConfigSection dockerConfigSection)
    {
      var containerConfigElement = dockerConfigSection.DockerContainer.Single(e => e.Name == name);
      if (name == "/crate")
      {
        return new CrateDatabaseInteractor(
          containerName: name,
          host: host,
          crateUrl: $"http://{host}:{containerConfigElement.Port}",
          shards: dockerConfigSection.DockerInstances.Count);
      }
      if (name == "/mongo")
      {
        return new MongoDatabaseInteractor(
          containerName: name,
          host: host);
      }

      return null;
    }
  }
}
