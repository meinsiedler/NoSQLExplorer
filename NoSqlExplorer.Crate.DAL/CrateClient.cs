using Newtonsoft.Json;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.Crate.DAL.Util;
using NoSqlExplorer.DAL.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

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
        var httpResponse = await client.PostAsync(this.GetRequestAddress(), content);
        var response = await this.HttpResponseToResponse(httpResponse);
        return response;
      }
    }

    public async Task<ICrateResponse> CreateTable<T>(int? shards = null, int? replicas = null)
    {
      var statement = StatementHelper.CreateTableStatement(typeof(T), shards, replicas);
      return await this.SubmitRequest(new CrateRequest(statement));
    }

    public async Task<ICrateResponse> Insert<T>(T entity)
    {
      var statement = StatementHelper.InsertStatement(entity);
      return await this.SubmitRequest(new CrateRequest(statement));
    }

    public async Task<ICrateResponse> BulkInsert<T>(IEnumerable<T> entities)
    {
      var statement = StatementHelper.BulkInsertStatement(entities);
      return await this.SubmitRequest(new CrateRequest(statement));
    }

    public async Task<ICrateResponse<T>> SubmitQuery<T>(string query)
    {
      using (var client = new HttpClient())
      {
        var request = new CrateRequest(query);
        var content = this.ObjectToHttpContent(request);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var httpResponse = await client.PostAsync(this.GetRequestAddress(), content);
        stopWatch.Stop();
        var response = await this.HttpResponseToResponse<T>(httpResponse, stopWatch.Elapsed);
        return response;
      }
    }

    public async Task<ICrateResponse> DropTable<T>()
    {
      var statement = $"drop table {Helper.GetTableName(typeof(T))}";
      return await this.SubmitRequest(new CrateRequest(statement));
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
      var responseContent = await response.Content.ReadAsStringAsync();

      if (response.StatusCode != System.Net.HttpStatusCode.OK)
      {
        return JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
      }
      else
      {
        return JsonConvert.DeserializeObject<SuccessResponse>(responseContent);
      }
    }

    private async Task<ICrateResponse<T>> HttpResponseToResponse<T>(HttpResponseMessage response, TimeSpan executionTime)
    {
      var responseContent = await response.Content.ReadAsStringAsync();
      ICrateResponse<T> result = null;
      if (response.StatusCode != System.Net.HttpStatusCode.OK)
      {
        result = JsonConvert.DeserializeObject<ErrorResponse<T>>(responseContent);
      }
      else
      {
        result = JsonConvert.DeserializeObject<SuccessResponse<T>>(responseContent);
        (result as SuccessResponse<T>).MapResult();
      }

      result.ExecutionTime = executionTime;
      return result;
    }
  }
}
