using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DatabaseInteraction.Queries;

namespace NoSqlExplorer.WpfClient.ViewModels.Queries
{
  public class GetAverageFollowersQueryViewModel : IQueryViewModel
  {
    public bool IsValid => true;
    public GetAverageFollowersQuery Query => new GetAverageFollowersQuery();
  }
}
