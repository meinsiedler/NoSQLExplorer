using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Utils
{
  public class TimestampConverter
  {
    public static DateTime TimestampToDateTime(long unixTimeMilliseconds)
    {
      var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return dateTime.AddMilliseconds(unixTimeMilliseconds);
    }
  }
}
