using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DAL.Common
{
  public static class Util
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
