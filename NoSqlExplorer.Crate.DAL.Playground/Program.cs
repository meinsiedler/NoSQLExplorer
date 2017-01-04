using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlExplorer.Crate.DAL.Playground
{
  class Program
  {
    static void Main(string[] args)
    {
      //Task.Factory.StartNew(() => CreateTableAsync());
      //Task.Factory.StartNew(() => CreateSchema());
      //Task.Factory.StartNew(() => Insert());
      Task.Factory.StartNew(() => BulkInsert());
      Console.ReadLine();
    }

    private static async Task CreateTableAsync()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var createTestTable = new CrateRequest("create table my_table (first_column integer, second_column string)");
      var response = await client.SubmitRequest(createTestTable);
      Console.WriteLine(response);
    }

    private static async Task CreateSchema()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var response = await client.CreateTable<Tweet>(4);
      Console.WriteLine(response);
    }

    private static async Task Insert()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var tweet1 = new Tweet { Content = "NoSqlExplorer rocks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 1, Retweeted = false };
      var tweet2 = new Tweet { Content = "NoSqlExplorer still rocks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 2, Retweeted = false };
      var response = await client.Insert(tweet1);
      var response2 = await client.Insert(tweet2);
      Console.WriteLine(response);
      Console.WriteLine(response2);
    }

    private static async Task BulkInsert()
    {
      var client = new CrateClient("http://clccontainer1.cloudapp.net:4200");
      var tweets = new List<Tweet>()
      {
        new Tweet { Content = "NoSqlExplorer rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 3, Retweeted = false },
        new Tweet { Content = "NoSqlExplorer still rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 4, Retweeted = false },
        new Tweet { Content = "NoSqlExplorer still rocks in bulks", DoubleProp = 13.37, FloatProp = 6.999999f, Id = 5, Retweeted = false },
      };
      var response = await client.BulkInsert(tweets);
      Console.WriteLine(response);
    }
  }
}
