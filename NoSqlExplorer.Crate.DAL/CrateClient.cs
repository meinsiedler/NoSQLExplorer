using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL
{
  public class CrateClient
  {
    private string connectionString;

    public CrateClient(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public async Task SubmitRequest(CrateRequest request)
    {
      using (var client = new HttpClient())
      {
        var result = client.PostAsync(this.GetRequestAddress(), this.ObjectToHttpContent(request))
      }
    }

    private string GetRequestAddress()
    {
      return $"{connectionString}/_sql?pretty";
    }

    private HttpContent ObjectToHttpContent(object obj)
    {
      var stringPayload = JsonConvert.SerializeObject(obj);
      return new StringContent(stringPayload, Encoding.UTF8, "application/json");
    }
  }
}
