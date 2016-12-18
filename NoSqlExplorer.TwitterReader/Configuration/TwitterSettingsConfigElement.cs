using System.Configuration;

namespace NoSqlExplorer.TwitterReader.Configuration
{
  public class TwitterSettingsConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("feedUrl")]
    public string FeedUrl => base["feedUrl"] as string;

    [ConfigurationProperty("consumerKey")]
    public string ConsumerKey => base["consumerKey"] as string;

    [ConfigurationProperty("consumerSecret")]
    public string ConsumerSecret => base["consumerSecret"] as string;
  }
}
