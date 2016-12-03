using System.Configuration;
using NoSqlExplorer.TwitterReader.Configuration;

namespace NoSqlExplorer.WpfClient.Configuration
{
  internal class AppConfigSettings : ITwitterConfigSettings
  {
    public string TwitterConsumerKey { get; } = GetAppSettingsValue("twitter:consumer-key");
    public string TwitterConsumerSecret { get; } = GetAppSettingsValue("twitter:consumer-secret");
    public string TwitterFeedUrl { get; } = GetAppSettingsValue("twitter:feed-url");

    private static string GetAppSettingsValue(string key)
    {
      return ConfigurationManager.AppSettings[key];
    }
  }
}
