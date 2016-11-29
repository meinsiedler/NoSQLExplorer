using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.WpfClient.Messages
{
  public class IsLoadingMessage
  {
    public bool IsLoading { get; set; }
    public string Reason { get; set; }
  }
}
