using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Mongo.DAL.Response
{
  public interface IMongoResponse
  {
    bool Success { get; }
    IEnumerable<string> Errors { get; }
  }

  public interface IMongoResponse<out T> : IMongoResponse
  {
    T Data { get; }
    double? ExecutionTime { get; }

  }
}
