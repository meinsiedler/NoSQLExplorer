using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.Crate.DAL.Util;

namespace NoSqlExplorer.Crate.DAL
{
  public class CrateClient
  {
    private string connectionString;

    public CrateClient(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public async Task<ICrateResponse> SubmitRequest(CrateRequest request)
    {
      using (var client = new HttpClient())
      {
        var content = this.ObjectToHttpContent(request);
        // TODO: Do timing here and add it to response
        var httpResponse = await client.PostAsync(this.GetRequestAddress(), content);
        // end timing
        var response = await this.HttpResponseToResponse(httpResponse);
        return response;
      }
    }

    public async Task<ICrateResponse> CreateTable<T>(int? shards = null, int? replicas = null) where T : class
    {
      var statement = StatementHelper.CreateTableStatement(typeof(T), shards, replicas);
      return await this.SubmitRequest(new CrateRequest(statement.ToString()));
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

    private async Task<ICrateResponse> HttpResponseToResponse(HttpResponseMessage response)
    {
      if (response.StatusCode != System.Net.HttpStatusCode.OK)
      {
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
      }

      // TODO: success
      return null;
    }
  }
}
