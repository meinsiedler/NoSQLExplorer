using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DockerAdapter.Configuration;

namespace NoSqlExplorer.TweetImporter
{
  public static class TweetImporterFactory
  {
    public static ITweetImporter CreateTweetImporter(string name, string host, DockerConfigSection dockerConfigSection)
    {
      var containerConfigElement = dockerConfigSection.DockerContainer.Single(e => e.Name == name);
      if (name == "/crate")
      {
        return new CrateTweetImporter(
          crateUrl: $"http://{host}:{containerConfigElement.Port}",
          shards: dockerConfigSection.DockerInstances.Count);
      }
      if (name == "/mongo")
      {
        return new MongoTweetImporter();
      }

      return null;
    }
  }
}
