using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DockerAdapter.ConfigSection
{
  public class DockerConfigSection : ConfigurationSection
  {
    public const string SectionName = "dockerConfig";

    [ConfigurationProperty("dockerInstances")]
    public DockerInstanceConfigCollection DockerInstances => base["dockerInstances"] as DockerInstanceConfigCollection;
  }
}
