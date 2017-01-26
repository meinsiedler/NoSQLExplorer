using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSqlExplorer.Crate.DAL.Response;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.DatabaseInteraction.Queries;

namespace NoSqlExplorer.Crate.DAL.Playground
{
  class Program
  {
    static void Main(string[] args)
    {
      //Task.Factory.StartNew(() => CreateTableAsync());
      //Task.Factory.StartNew(() => CreateSchema());
      //Task.Factory.StartNew(() => Insert());
      //Task.Factory.StartNew(() => BulkInsert());
      //Task.Factory.StartNew(() => Query());
      //Task.Factory.StartNew(() => RunAll());

      Task.Factory.StartNew(() => RunQuery());

      Console.ReadLine();
    }

    private static async Task RunAll()
    {
      await DropSchema();
      await CreateSchema();
      await Insert();
      await BulkInsert();
      await Query();
    }

    private static async Task DropSchema()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var response = await client.DropTable<Tweet>();
      Console.WriteLine("DropSchema");
      Console.WriteLine(response);
    }

    private static async Task CreateSchema()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var response = await client.CreateTable<Tweet>(4);
      Console.WriteLine("CreateSchema");
      Console.WriteLine(response);
    }

    private static async Task Insert()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var tweet1 = new Tweet { Content = "NoSqlExplorer rocks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 1, Retweeted = false, Created = DateTime.UtcNow };
      var tweet2 = new Tweet { Content = "NoSqlExplorer still rocks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 2, Retweeted = false, Created = DateTime.UtcNow };
      var response = await client.Insert(tweet1);
      var response2 = await client.Insert(tweet2);
      Console.WriteLine("Insert(1)");
      Console.WriteLine(response);
      Console.WriteLine("Insert(2)");
      Console.WriteLine(response2);
    }

    private static async Task BulkInsert()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var tweets = new List<Tweet>()
      {
        new Tweet { Content = "NoSqlExplorer rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 13, Retweeted = false, Created = DateTime.UtcNow },
        new Tweet { Content = "NoSqlExplorer still rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 14, Retweeted = false, Created = DateTime.UtcNow },
        new Tweet { Content = "NoSqlExplorer still rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 15, Retweeted = false, Created = DateTime.UtcNow },
      };
      var response = await client.BulkInsert(tweets);
      Console.WriteLine("BulkInsert");
      Console.WriteLine(response);
    }

    private static async Task Query()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var statement = "select * from tweeeeets where id >= 14";
      var response = await client.SubmitQuery<Tweet>(statement) as SuccessResponse<Tweet>;
      Console.WriteLine("Query");
      Console.WriteLine(response);
      if (response.Result == null)
      {
        return;
      }

      foreach (var entry in response.Result)
      {
        Console.WriteLine(entry);
      }
    }

    private static async Task RunQuery()
    {
      IDatabaseInteractor dbInteractor = new CrateDatabaseInteractor("/crate", "clccontainer1.cloudapp.net", "http://clccontainer1.cloudapp.net:4200", 4);

      try
      {
        var tweetsResult = await dbInteractor.GetQueryResultAsync(new GetTweetsWithHashtagQuery("#tweet"));
        tweetsResult.Result?.Take(10).ToList().ForEach(Console.WriteLine);
        Console.WriteLine($"Duration: {tweetsResult.DurationMillis} ms");

        var avgFollowersResult = await dbInteractor.GetQueryResultAsync(new GetAverageFollowersQuery());
        Console.WriteLine($"AVG Followers: {avgFollowersResult}");
        Console.WriteLine($"Duration: {avgFollowersResult.DurationMillis} ms");
      }
      catch (DatabaseException ex)
      {
        Console.WriteLine($"Database Exception : {ex.Message}.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"General Exception : {ex.Message}.");
      }
    }
  }
}
