using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.AzureAdapter
{
  public class CommandResponse
  {
    public CommandResponse(bool success, string error = null)
    {
      this.Success = success;
      this.Error = error;
    }

    public bool Success { get; private set; }
    public string Error { get; private set; }
  }
}
