using System;
using System.Reflection;

namespace NoSqlExplorer.DAL.Common
{
  public static class Helper
  {
    public static string GetTableName(Type type)
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
  }
}
