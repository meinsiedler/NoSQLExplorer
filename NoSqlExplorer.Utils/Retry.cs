using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Utils
{
  public static class Retry
  {
    public static async Task<T> TryAwait<T, TException>(Func<Task<T>> taskFunc, int maxTries = 5) where TException : Exception
    {
      var tries = 0;
      var task = taskFunc();
      for (;;)
      {
        try
        {
          return await task;
        }
        catch (TException)
        {
          tries++;
          if (tries >= maxTries)
            throw;

          task = taskFunc();
        }
      }
    }
  }
}
