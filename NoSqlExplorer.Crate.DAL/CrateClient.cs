﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        var httpResponse = await client.PostAsync(this.GetRequestAddress(), content);
        var response = await this.HttpResponseToResponse(httpResponse);
        return response;
      }
    }

    public async Task<ICrateResponse> CreateTable<T>(int? shards = null, int? replicas = null) where T : class
    {
      var statement = StatementHelper.CreateTableStatement(typeof(T), shards, replicas);
      return await this.SubmitRequest(new CrateRequest(statement));
    }

    public async Task<ICrateResponse> Insert<T>(T entity) where T : class
    {
      var statement = StatementHelper.InsertStatement(entity);
      return await this.SubmitRequest(new CrateRequest(statement));
    }

    public async Task<ICrateResponse> BulkInsert<T>(IEnumerable<T> entities) where T : class
    {
      var statement = StatementHelper.BulkInsertStatement(entities);
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
  }
}
