using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.DAL.Common
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
  public class TableNameAttribute : Attribute
  {
    public TableNameAttribute(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException("non-Empty table name required");
      }
      this.Name = name;
    }

    public virtual string Name { get; private set; }
  }
}
