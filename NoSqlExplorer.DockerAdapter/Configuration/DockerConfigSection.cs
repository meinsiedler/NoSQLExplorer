using System.Configuration;

namespace NoSqlExplorer.DockerAdapter.Configuration
{
  public class DockerConfigSection : ConfigurationSection
  {
    public const string SectionName = "dockerConfig";

    [ConfigurationProperty("dockerInstances")]
    public DockerInstanceConfigCollection DockerInstances => base["dockerInstances"] as DockerInstanceConfigCollection;

    [ConfigurationProperty("dockerContainer")]
    public DockerContainerConfigCollection DockerContainer => base["dockerContainer"] as DockerContainerConfigCollection;
  }
}
