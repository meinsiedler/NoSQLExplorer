using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DatabaseInteraction;

namespace NoSqlExplorer.WpfClient.ViewModels.QueryResults
{
  public class GetAverageFollowersQueryResultViewModel : ViewModelBase, IQueryResultRowViewModel
  {
    public GetAverageFollowersQueryResultViewModel(string hashtag, QueryResult<double> queryResult)
    {
      Hashtag = !string.IsNullOrEmpty(hashtag) ? hashtag : "<all tweets>" ;
      AverageFollowers = queryResult.Result;
      DurationMillis = queryResult.DurationMillis.GetValueOrDefault();
    }

    private string _hashtag;
    public string Hashtag
    {
      get { return _hashtag; }
      set { Set(ref _hashtag, value); }
    }

    private double _averageFollowers;
    public double AverageFollowers
    {
      get { return _averageFollowers; }
      set { Set(ref _averageFollowers, value); }
    }

    private double _durationMillis;
    public double DurationMillis
    {
      get { return _durationMillis; }
      set { Set(ref _durationMillis, value); }
    }
  }
}
