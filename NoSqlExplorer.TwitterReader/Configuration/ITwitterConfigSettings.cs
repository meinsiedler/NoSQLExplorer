namespace NoSqlExplorer.TwitterReader.Configuration
{
  public interface ITwitterConfigSettings
  {
    string TwitterConsumerKey { get; }
    string TwitterConsumerSecret { get; }
    string TwitterFeedUrl { get; }
  }
}
