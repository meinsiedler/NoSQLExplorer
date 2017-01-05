using System.Configuration;

namespace NoSqlExplorer.DockerAdapter.Configuration
{
  public class DockerContainerConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("name")]
    public string Name => base["name"] as string;

    [ConfigurationProperty("port")]
    public int Port => (int) base["port"];

    public override string ToString()
    {
      return $"Host: {Name}, Port: {Port}";
    }
  }
}
