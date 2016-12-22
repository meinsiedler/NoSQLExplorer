using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.AzureAdapter.Configuration
{
  public class AzureConfigSection : ConfigurationSection
  {
    public const string SectionName = "azureConfig";

    [ConfigurationProperty("azureSubscription")]
    public AzureSubscriptionConfigElement AzureSubscription => base["azureSubscription"] as AzureSubscriptionConfigElement;
  }
}
