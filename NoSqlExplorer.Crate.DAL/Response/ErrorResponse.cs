using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoSqlExplorer.Crate.DAL.Response
{
  public class ErrorResponse : ICrateResponse
  {
    [JsonProperty("error")]
    public Data.Error Error { get; set; }

    public override string ToString()
    {
      return $"Code: {this.Error.Code}; Error: {this.Error.Message}";
    }
  }

  public class ErrorResponse<T> : ErrorResponse, ICrateResponse<T> where T : class
  {
  }
}
