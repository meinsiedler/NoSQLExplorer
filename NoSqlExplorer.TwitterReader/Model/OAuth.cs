﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace NoSqlExplorer.TwitterReader.Model
{
  public class OAuth
  {
    static OAuth()
    {
      ServicePointManager.Expect100Continue = false;
    }

    public OAuth(string consumerKey, string consumerSecret)
    {
      ConsumerKey = consumerKey;
      ConsumerSecret = consumerSecret;
    }

    private string ConsumerKey { get; }
    private string ConsumerSecret { get; }

    public static string UrlEncode(string value)
    {
      return Uri.EscapeDataString(value);
    }

    public string Nonce()
    {
      return Guid.NewGuid().ToString();
    }

    public string TimeStamp()
    {
      var timespan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      return Convert.ToInt64(timespan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
    }

    public string Signature(string httpMethod, string url, string nonce, string timestamp, string accessToken, string accessTokenSecret, IEnumerable<string[]> parameters)
    {
      var parameterList = OrderedParameters(nonce, timestamp, accessToken, null, parameters);
      var parameterStrings = parameterList.Select(p => $"{p.Item1}={p.Item2}");
      var parameterString = string.Join("&", parameterStrings);
      var signatureBaseString = $"{httpMethod}&{UrlEncode(url)}&{UrlEncode(parameterString)}";
      var compositeKey = $"{UrlEncode(ConsumerSecret)}&{UrlEncode(accessTokenSecret)}";
      using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(compositeKey)))
      {
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString)));
      }
    }

    public string AuthorizationHeader(string nonce, string timestamp, string accessToken, string signature, IEnumerable<string[]> parameters = null)
    {
      var parameterList = OrderedParameters(nonce, timestamp, accessToken, signature, parameters);
      var parameterStrings = parameterList.Select(p => $"{p.Item1}=\"{p.Item2}\"");
      var header = "OAuth " + string.Join(",", parameterStrings);
      return header;
    }

    private IEnumerable<Tuple<string, string>> OrderedParameters(string nonce, string timestamp, string accessToken, string signature, IEnumerable<string[]> parameters)
    {
      var components = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("oauth_version", "1.0"),
                new Tuple<string, string>("oauth_nonce", UrlEncode(nonce)),
                new Tuple<string, string>("oauth_timestamp", UrlEncode(timestamp)),
                new Tuple<string, string>("oauth_signature_method", "HMAC-SHA1"),
                new Tuple<string, string>("oauth_consumer_key", UrlEncode(ConsumerKey))
            };

      if (string.IsNullOrWhiteSpace(signature) == false)
      {
        components.Add(new Tuple<string, string>("oauth_signature", UrlEncode(signature)));
      }
      if (string.IsNullOrWhiteSpace(accessToken) == false)
      {
        components.Add(new Tuple<string, string>("oauth_token", UrlEncode(accessToken)));
      }
      if (parameters != null)
      {
        components.AddRange(parameters.Select(par => new Tuple<string, string>(UrlEncode(par[0]), UrlEncode(par[1]))));
      }
      return components.OrderBy(c => c.Item1);
    }
  }
}
