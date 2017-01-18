using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.Mongo.DAL.Playground
{
  class Program
  {
    static void Main(string[] args)
    {
      //Task.Factory.StartNew(() => Insert());
      //Task.Factory.StartNew(() => BulkInsert());
      Task.Factory.StartNew(() => Query());
      //Task.Factory.StartNew(() => RunAll());
      Console.ReadLine();
    }

    private static async Task Query()
    {
      var client = new MongoDbClient("dockerplayground.cloudapp.net", 27017, "...", "...", "replTestDb");
      var single = await client.Single<Tweet>(t => t.Text.Contains("999"));
      Console.WriteLine($"{single.Success} => {single.Data?.Text}");
      var multi = await client.FindAsync<Tweet>(t => t.Id > 9950);
      Console.WriteLine($"{multi.Success} => count: {multi.Data.Count()}, {multi.Data.FirstOrDefault()?.Text}");
    }

    private static async Task Insert()
    {
      var client = new MongoDbClient("dockerplayground.cloudapp.net", 27017, "mdi1984", "password", "replTestDb");
      for(int i = 0; i < 10000; i++)
      {
        var response = await client.AddAsync(new Tweet(i, $"tweet{i} content", "someSource", i, 10, DateTime.Now));
        if (!response.Success)
        {
          Console.WriteLine(response);
        }
      }
    }
  }
}
