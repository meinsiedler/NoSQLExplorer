using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.AzureAdapter.Configuration
{
  public class AzureSubscriptionConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("subscriptionId")]
    public string SubscriptionId => base["subscriptionId"] as string;

    [ConfigurationProperty("resourceGroup")]
    public string ResourceGroup => base["resourceGroup"] as string;

    [ConfigurationProperty("base64encodedCertificate")]
    public string Base64encodedCertificate => base["base64encodedCertificate"] as string;
  }
}
