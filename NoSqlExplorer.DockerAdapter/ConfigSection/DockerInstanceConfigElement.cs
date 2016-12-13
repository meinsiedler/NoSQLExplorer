using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DockerAdapter.ConfigSection
{
  public class DockerInstanceConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("host")]
    public string Host => base["host"] as string;

    [ConfigurationProperty("port")]
    public int Port => (int) base["port"];

    [ConfigurationProperty("username")]
    public string Username => base["username"] as string;

    [ConfigurationProperty("password")]
    public string Password => base["password"] as string;

    public override string ToString()
    {
      return $"Host: {Host}, Port: {Port}, Username: {Username}, Password: {Password}";
    }
  }
}
