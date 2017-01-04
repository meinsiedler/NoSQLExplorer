using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.DAL.Common;

namespace NoSqlExplorer.Crate.DAL.Playground
{
  [TableName("tweeeeets")]
  public class Tweet
  {
    [PrimaryKey]
    public int Id { get; set; }
    public string Content { get; set; }
    public bool Retweeted { get; set; }
    public float FloatProp { get; set; }
    public double DoubleProp { get; set; }
  }
}
