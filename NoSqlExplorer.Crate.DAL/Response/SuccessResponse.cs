using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL.Response
{
  public class SuccessResponse : ICrateResponse
  {
    public int RowCount { get; set; }
    public double Duration { get; set; }

    public override string ToString()
    {
      return $"Success -  RowCount: {this.RowCount}; Duration: {this.Duration}";
    }
  }
}
