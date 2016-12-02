using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoSqlExplorer.TwitterReader.Tests
{
  /// <summary>
  /// Note that these tests rely on an available internet connection since they query public URLs.
  /// </summary>
  [TestFixture]
  public class TwitterReaderTests
  {
    [Test]
    public async Task GetLinesFromSimpleDomain()
    {
      ITwitterReader reader = new TwitterReader("http://stackoverflow.com");

      var lines = new List<string>();

      reader.OnNewTweet += line => lines.Add(line);
      await reader.StartAsync("", "");
      await Task.Delay(20);

      Assert.That(reader.IsRunning);

      await Task.Delay(100);
      reader.Stop();
      await Task.Delay(20);

      Assert.That(reader.IsRunning, Is.False);

      Assert.That(lines, Is.Not.Empty);
    }

    [Test]
    public void GetAuthError()
    {
      ITwitterReader reader = new TwitterReader("https://twitter.crate.io/api/v1/sample");

      var ex = Assert.Throws<WebException>(async () => await reader.StartAsync("", ""));
      Assert.That(ex.Message, Contains.Substring("(401)"));
    }
  }
}
