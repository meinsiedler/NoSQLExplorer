using System.Configuration;

namespace NoSqlExplorer.TwitterReader.Configuration
{
  public class TwitterConfigSection : ConfigurationSection
  {
    public const string SectionName = "twitterConfig";

    [ConfigurationProperty("twitterSettings")]
    public TwitterSettingsConfigElement TwitterSettings => base["twitterSettings"] as TwitterSettingsConfigElement;
  }
}
