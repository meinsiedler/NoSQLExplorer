using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL
{
  public class CrateRequest
  {
    public CrateRequest(string statement, params object[] args)
    {
      this.Statement = statement;
      this.Args = args;
    }

    public string Statement { get; private set; }
    public object[] Args { get; private set; }
  }
}
