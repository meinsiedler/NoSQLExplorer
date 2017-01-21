using System.Collections.Generic;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public class GetTweetsWithHashtagQueryResultViewModel : ViewModelBase, IQueryResultRowViewModel
  {
    public GetTweetsWithHashtagQueryResultViewModel(string hashtag, QueryResult<IList<Tweet>> queryResult)
    {
      Hashtag = hashtag;
      TweetCount = queryResult.Result.Count;
      DurationMillis = queryResult.DurationMillis;
    }

    private string _hashtag;
    public string Hashtag
    {
      get { return _hashtag; }
      set { Set(ref _hashtag, value); }
    }

    private int _tweetCount;
    public int TweetCount
    {
      get { return _tweetCount; }
      set { Set(ref _tweetCount, value); }
    }

    private double _durationMillis;
    public double DurationMillis
    {
      get { return _durationMillis; }
      set { Set(ref _durationMillis, value); }
    }
  }
}
