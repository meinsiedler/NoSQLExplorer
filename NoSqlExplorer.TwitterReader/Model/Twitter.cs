using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.TwitterReader.Configuration;

namespace NoSqlExplorer.TwitterReader.Model
{
  public static class Twitter
  {
    private static async Task<T> RequestHandler<T>(Func<Task<T>> request)
    {
      return await request();
    }

    public static async Task<OAuthTokens> GetRequestToken(string twitterConsumerKey, string twitterConsumerSecret)
    {
      return await RequestHandler(async () =>
      {
        const string requestTokenUrl = "https://api.twitter.com/oauth/request_token";
        var oauth = OAuthFactory.CreateOAuth(twitterConsumerKey, twitterConsumerSecret);
        var nonce = oauth.Nonce();
        var timestamp = oauth.TimeStamp();
        var parameters = new[] { new[] { "oauth_callback", "oob" } };
        var signature = oauth.Signature("POST", requestTokenUrl, nonce, timestamp, "", "", parameters);
        var authorizationHeader = oauth.AuthorizationHeader(nonce, timestamp, null, signature, parameters);

        var request = System.Net.WebRequest.Create(new Uri(requestTokenUrl));
        request.Method = "POST";
        request.Headers.Add("Authorization", authorizationHeader);
        using (var response = await request.GetResponseAsync())
        using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        {
          var body = stream.ReadToEnd();
          var tokens = body.Split('&');
          var oauthToken = Token(tokens[0]);
          var oauthSecret = Token(tokens[1]);
          var callbackConfirmed = Token(tokens[2]);

          if (callbackConfirmed != "true") throw new InvalidProgramException("callback token not confirmed");
          return new OAuthTokens { OAuthToken = oauthToken, OAuthSecret = oauthSecret };
        }
      });
    }

    public static string GetRequestUrl(OAuthTokens tokens)
    {
      return "https://api.twitter.com/oauth/authenticate?oauth_token=" + tokens.OAuthToken;
    }

    public static async Task<OAuthTokens> GetAccessToken(string twitterConsumerKey, string twitterConsumerSecret, string accessToken, string accessTokenSecret, string oauthVerifier)
    {
      return await RequestHandler(async () =>
      {
        const string requestTokenUrl = "https://api.twitter.com/oauth/access_token";
        var oauth = OAuthFactory.CreateOAuth(twitterConsumerKey, twitterConsumerSecret);
        var nonce = oauth.Nonce();
        var timestamp = oauth.TimeStamp();
        var parameters = new[] { new[] { "oauth_verifier", oauthVerifier } };
        var signature = oauth.Signature("POST", requestTokenUrl, nonce, timestamp, accessToken, accessTokenSecret, parameters);
        var authorizationHeader = oauth.AuthorizationHeader(nonce, timestamp, accessToken, signature, parameters);

        var request = System.Net.WebRequest.Create(new Uri(requestTokenUrl));
        request.Method = "POST";
        request.Headers.Add("Authorization", authorizationHeader);

        using (var response = await request.GetResponseAsync())
        using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        {
          var tokens = stream.ReadToEnd().Split('&');
          var oauthTokens = new OAuthTokens
          {
            OAuthToken = Token(tokens[0]),
            OAuthSecret = Token(tokens[1]),
            UserId = Token(tokens[2]),
          };
          return oauthTokens;
        }
      });
    }

    private static string Token(string pair)
    {
      return pair.Substring(pair.IndexOf('=') + 1);
    }
  }
}
