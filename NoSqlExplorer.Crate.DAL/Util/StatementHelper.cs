using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL.Util
{
  public static class StatementHelper
  {
    public static string CreateTableStatement(Type type, int? shards = null, int? replicas = null)
    {
      var props = type.GetProperties();
      if (props.Length == 0)
      {
        throw new InvalidOperationException($"Class {type.Name} does not contain any valid properties");
      }

      var columnInfo = GetTypedColumnInfo(props);

      var statement = new StringBuilder($"create table {type.Name.ToLower()} (");
      foreach (var column in columnInfo)
      {
        statement.Append($"{column.Key} {column.Value}");
        if (column.Key != columnInfo.Last().Key)
        {
          statement.Append(", ");
        }
      }
      statement.Append(")");


      if (shards != null)
      {
        statement.Append($" clustered into {shards} shards");
      }

      if (replicas != null)
      {
        statement.Append($" with (number_of_replicas = {replicas})");
      }
      return statement.ToString();
    }

    private static Dictionary<string, string> GetTypedColumnInfo(PropertyInfo[] properties)
    {
      var result = new Dictionary<string, string>();
      foreach (var property in properties)
      {
        var type = "string";
        switch (property.PropertyType.Name)
        {
          case nameof(Int32):
            type = "integer";
            break;
          case nameof(Boolean):
            type = "boolean";
            break;
          case nameof(Double):
            type = "double";
            break;
          case nameof(Single):
            type = "float";
            break;
          default:
            break;
        }

        result.Add(property.Name, type);
      }

      return result;
    }
  }
}
