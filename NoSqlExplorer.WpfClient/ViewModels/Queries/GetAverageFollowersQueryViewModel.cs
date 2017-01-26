using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DatabaseInteraction.Queries;

namespace NoSqlExplorer.WpfClient.ViewModels.Queries
{
  public class GetAverageFollowersQueryViewModel : ViewModelBase, IQueryViewModel
  {
    public string QueryName => "Get Average Followers";

    public bool IsValid => true;

    private string _hashtag;
    public string Hashtag
    {
      get { return _hashtag; }
      set
      {
        Set(ref _hashtag, value);
        Query = !string.IsNullOrEmpty(_hashtag) ? new GetAverageFollowersQuery($"#{_hashtag}") : new GetAverageFollowersQuery();
        RaisePropertyChanged(() => IsValid);
      }
    }

    public GetAverageFollowersQuery Query { get; private set; } = new GetAverageFollowersQuery();
  }
}
