using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoSqlExplorer.Crate.DAL
{
  public class CrateRequest
  {
    public CrateRequest(string statement, params object[] args)
    {
      this.Statement = statement;
      this.Args = args;
    }

    [JsonProperty("stmt")]
    public string Statement { get; private set; }

    [JsonProperty("args")]
    public object[] Args { get; private set; }
  }
}
