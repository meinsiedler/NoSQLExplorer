using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NoSqlExplorer.Crate.DAL.Response
{
  public class SuccessResponse : ICrateResponse
  {
    public int RowCount { get; set; }
    public double Duration { get; set; }

    public override string ToString()
    {
      return $"Success -  RowCount: {this.RowCount}; Duration: {this.Duration}";
    }
  }

  public class SuccessResponse<T> : SuccessResponse, ICrateResponse<T> where T : class
  {
    [JsonProperty("cols")]
    internal string[] Cols { get; set; }

    [JsonProperty("rows")]
    internal string[][] Rows { get; set; }

    public List<T> Result { get; set; }

    internal SuccessResponse<T> MapResult()
    {
      if (this.Cols.Length > 0 && this.Rows.Length > 0)
      {
        var type = typeof(T);
        var properties = type.GetProperties().ToDictionary(p => p.Name.ToLower(), p => p);
        this.Result = new List<T>();

        foreach (var row in this.Rows)
        {
          var instance = Activator.CreateInstance<T>();
          for (int i = 0; i < this.Cols.Length; i++)
          {
            var propInfo = properties[this.Cols[i].ToLower()];
            propInfo.SetValue(instance, Convert.ChangeType(row[i], propInfo.PropertyType));
          }

          this.Result.Add(instance);
        }
      }

      return this;
    }
  }
}
