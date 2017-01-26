using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.TwitterReader.Configuration;

namespace NoSqlExplorer.TwitterReader.Model
{
  public static class OAuthFactory
  {
    public static OAuth CreateOAuth(string twitterConsumerKey, string twitterConsumerSecret)
    {
      return new OAuth(twitterConsumerKey, twitterConsumerSecret);
    }
  }
}
