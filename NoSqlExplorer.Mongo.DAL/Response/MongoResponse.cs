using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace NoSqlExplorer.Mongo.DAL.Response
{
  public class MongoResponse : IMongoResponse
  {
    public MongoResponse()
    {
    }

    public MongoResponse(DeleteResult deleteResult)
    {
      this.Success = deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
      if (deleteResult.DeletedCount == 0)
      {
        this.Errors = new List<string>() { "no matching entries found" };
      }
    }

    public MongoResponse(Exception exception)
    {
      this.Success = false;
      this.Errors = new List<string>() { exception.Message };
    }

    public MongoResponse(bool result)
    {
      this.Success = result;
    }

    public MongoResponse(ReplaceOneResult replaceResult)
    {
      if (replaceResult.IsAcknowledged)
      {
        if (replaceResult.MatchedCount == 0)
        {
          this.Success = false;
          this.Errors = new List<string>() { "no matching entries found" };
        }
        else if (replaceResult.ModifiedCount != 1)
        {
          this.Success = false;
          this.Errors = new List<string>() { "entry not changed" };
        }
        else
        {
          this.Success = true;
        }
      }
      else
      {
        this.Success = false;
        this.Errors = new List<string>() { "not acknowledged" };
      }
    }

    public bool Success { get; private set; }
    public IEnumerable<string> Errors { get; set; }
  }

  public class MongoResponse<T> : IMongoResponse<T>
  {
    public MongoResponse(T result)
    {
      if (result != null)
      {
        this.Data = result;
        this.Success = true;
      }
      else
      {
        this.Success = false;
        this.Errors = new List<string>() { "no results found" };
      }
    }

    public double? ExecutionTime { get; set; }

    public MongoResponse(Exception ex)
    {
      this.Success = false;
      this.Errors = new List<string>() { ex.Message };
    }

    public T Data { get; private set; }
    public bool Success { get; }
    public IEnumerable<string> Errors { get; }
  }
}
