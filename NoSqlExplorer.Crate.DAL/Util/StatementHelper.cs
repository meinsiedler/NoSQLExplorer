using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DAL.Common;

namespace NoSqlExplorer.Crate.DAL.Util
{
  internal static class StatementHelper
  {
    internal static string CreateTableStatement(Type type, int? shards = null, int? replicas = null)
    {
      var props = type.GetProperties();
      if (props.Length == 0)
      {
        throw new InvalidOperationException($"Class {type.Name} does not contain any valid properties");
      }

      var statement = new StringBuilder($"create table {GetTableName(type)} (");
      foreach (var column in props)
      {
        var crateType = GetCrateColumnType(column.PropertyType);
        statement.Append($"{column.Name} {crateType}");

        if (column.GetCustomAttribute(typeof(PrimaryKeyAttribute)) != null)
        {
          statement.Append(" primary key");
        }

        if (column.Name != props.Last().Name)
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

    internal static string InsertStatement<T>(T entity)
    {
      var type = typeof(T);
      var propertyNames = GetPropertyNames(type);
      var statement = new StringBuilder($"insert into {GetTableName(type)} ({string.Join(",", propertyNames)}) values ");
      statement.Append($"({string.Join(",", GetPropertyValues(propertyNames, entity))})");

      return statement.ToString();
    }

    internal static string BulkInsertStatement<T>(IEnumerable<T> entities)
    {
      var type = typeof(T);
      var propertyNames = GetPropertyNames(type);
      var statement = new StringBuilder($"insert into {GetTableName(type)} ({string.Join(",", propertyNames)}) values ");
      foreach (var entity in entities)
      {
        statement.Append($"({string.Join(",", GetPropertyValues(propertyNames, entity))}),");
      }

      //remove last ,
      statement.Remove(statement.Length - 1, 1);
      return statement.ToString();
    }

    internal static string GetTableName(Type type)
    {
      var tableNameAttr = type.GetCustomAttribute<TableNameAttribute>();
      if (tableNameAttr != null)
      {
        return tableNameAttr.Name;
      }
      else
      {
        return type.Name.ToLower();
      }
    }

    private static IEnumerable<string> GetPropertyValues<T>(IEnumerable<string> propertyNames, T entity)
    {
      foreach (var property in propertyNames)
      {
        var value = entity.GetType().GetProperty(property).GetValue(entity, null);
        var valueType = value.GetType();
        if (valueType == typeof(string))
        {
          yield return $"'{JavaScriptStringEncode(value.ToString(), false)}'";
        }
        else if (valueType == typeof(DateTime))
        {
          yield return $"'{JavaScriptStringEncode(((DateTime)value).ToString("o"), false)}'";
        }
        else
        {
          yield return JavaScriptStringEncode(value.ToString().Replace(',', '.'), false);
        }
      }
    }

    private static IEnumerable<string> GetPropertyNames(Type type)
    {
      return type.GetProperties().Select(prop => prop.Name);
    }


    private static string GetCrateColumnType(Type propertyType)
    {
      var type = "string";
      switch (propertyType.Name)
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
        case nameof(DateTime):
          type = "timestamp";
          break;
        default:
          break;
      }

      return type;
    }

    private static string JavaScriptStringEncode(string value, bool addDoubleQuotes)
    {
      if (string.IsNullOrEmpty(value))
        return addDoubleQuotes ? "\"\"" : string.Empty;

      int len = value.Length;
      bool needEncode = false;
      char c;
      for (int i = 0; i < len; i++)
      {
        c = value[i];

        if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
        {
          needEncode = true;
          break;
        }
      }

      if (!needEncode)
        return addDoubleQuotes ? "\"" + value + "\"" : value;

      var sb = new System.Text.StringBuilder();
      if (addDoubleQuotes)
        sb.Append('"');

      for (int i = 0; i < len; i++)
      {
        c = value[i];
        if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
          sb.AppendFormat("\\u{0:x4}", (int)c);
        else switch ((int)c)
          {
            case 8:
              sb.Append("\\b");
              break;

            case 9:
              sb.Append("\\t");
              break;

            case 10:
              sb.Append("\\n");
              break;

            case 12:
              sb.Append("\\f");
              break;

            case 13:
              sb.Append("\\r");
              break;

            case 34:
              sb.Append("\\\"");
              break;

            case 92:
              sb.Append("\\\\");
              break;

            default:
              sb.Append(c);
              break;
          }
      }

      if (addDoubleQuotes)
        sb.Append('"');

      return sb.ToString();
    }
  }
}
