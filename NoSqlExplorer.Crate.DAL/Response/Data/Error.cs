using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoSqlExplorer.Crate.DAL.Response.Data
{
  public class Error
  {
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("code")]
    public int Code { get; set; }
  }
}
