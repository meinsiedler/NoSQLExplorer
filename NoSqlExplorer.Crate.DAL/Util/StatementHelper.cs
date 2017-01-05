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

    internal static string InsertStatement<T>(T entity) where T : class
    {
      var type = typeof(T);
      var properties = type.GetProperties();

      var propertyNames = GetPropertyNames(type);
      var statement = new StringBuilder($"insert into {GetTableName(type)} ({string.Join(",", propertyNames)}) values ");
      statement.Append($"({string.Join(",", GetPropertyValues(propertyNames, entity))})");

      return statement.ToString();
    }

    internal static string BulkInsertStatement<T>(IEnumerable<T> entities) where T : class
    {
      var type = typeof(T);
      var properties = type.GetProperties();

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

    private static IEnumerable<string> GetPropertyValues<T>(IEnumerable<string> propertyNames, T entity) where T : class
    {
      foreach (var property in propertyNames)
      {
        var value = entity.GetType().GetProperty(property).GetValue(entity, null);
        var valueType = value.GetType();
        if (valueType == typeof(string))
        {
          yield return $"'{value.ToString()}'";
        }
        else if (valueType == typeof(DateTime))
        {
          // TODO - format to iso stuff.. 
        }
        else
        {
          yield return value.ToString().Replace(',', '.');
        }
      }
    }

    private static IEnumerable<string> GetPropertyNames(Type type)
    {
      var props = type.GetProperties();
      foreach (var prop in props)
      {
        yield return prop.Name;
      }
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
        default:
          break;
      }

      return type;
    }
  }
}
