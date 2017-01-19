using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.WpfClient.ViewModels.Queries
{
  public class GetTweetsWithHashtagQueryViewModel : ViewModelBase, IQueryViewModel
  {
    private string _hashtag;

    public string Hashtag
    {
      get { return _hashtag; }
      set
      {
        Set(ref _hashtag, value);
        if (!string.IsNullOrEmpty(_hashtag))
        {
          Query = new GetTweetsWithHashtagQuery($"#{_hashtag}");
        }
        RaisePropertyChanged(() => IsValid);
      }
    }

    public bool IsValid => !string.IsNullOrEmpty(Hashtag);

    public GetTweetsWithHashtagQuery Query { get; private set; }
    
  }
}
