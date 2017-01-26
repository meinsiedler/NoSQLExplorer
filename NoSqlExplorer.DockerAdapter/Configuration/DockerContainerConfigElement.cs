using System.Configuration;

namespace NoSqlExplorer.DockerAdapter.Configuration
{
  public class DockerContainerConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("name")]
    public string Name => base["name"] as string;

    [ConfigurationProperty("port")]
    public int Port => (int) base["port"];

    [ConfigurationProperty("username")]
    public string Username => base["username"] as string;

    [ConfigurationProperty("password")]
    public string Password => base["password"] as string;

    public override string ToString()
    {
      return $"Host: {Name}, Port: {Port}";
    }
  }
}
