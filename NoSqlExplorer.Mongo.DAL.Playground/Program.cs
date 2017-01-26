﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using NoSqlExplorer.DatabaseInteraction;
using NoSqlExplorer.DatabaseInteraction.Queries;
using NoSqlExplorer.Twitter.Common;

namespace NoSqlExplorer.Mongo.DAL.Playground
{
  class Program
  {
    static void Main(string[] args)
    {
      //Task.Factory.StartNew(() => Insert());
      //Task.Factory.StartNew(() => BulkInsert());
      //Task.Factory.StartNew(() => Query());
      //Task.Factory.StartNew(() => RunAll());
      //Task.Factory.StartNew(() => PredefinedQueries());
      Task.Factory.StartNew(() => AggregateQuery());

      Console.ReadLine();
    }

    private static MongoDbClient CreateClient()
    {
      return new MongoDbClient("dockerplayground.cloudapp.net", 27017, "mdi1984", "password", "TweetCollection");
    }

    private static async Task Query()
    {
      var client = CreateClient();
      var single = await client.Single<Tweet>(t => t.Text.Contains("999"));
      Console.WriteLine($"{single.Success} => {single.Data?.Text}");
      var multi = await client.FindAsync<Tweet>(t => t.Id > 9950);
      Console.WriteLine($"{multi.Success} => count: {multi.Data.Count()}, {multi.Data.FirstOrDefault()?.Text}");
    }

    private static async Task AggregateQuery()
    {
      var client = CreateClient();
      var groupingDoc = new BsonDocument { { "_id", "null" }, { "avgFollowers", new BsonDocument("$avg", "$Followers") } };
      var aggregateResult = await client.Aggregate<Tweet>(new BsonDocument(), groupingDoc);
      Console.WriteLine($"avgFollowers: {aggregateResult.Data.First()["avgFollowers"].AsDouble} took {aggregateResult.ExecutionTime} ms");

      var trumpMatch = new BsonDocument { { "$text", new BsonDocument("$search", "#Trump") } };
      var aggregateTrumpResult = await client.Aggregate<Tweet>(trumpMatch, groupingDoc);
      Console.WriteLine($"avgFollowers #Trump: {aggregateTrumpResult.Data.First()["avgFollowers"].AsDouble} took {aggregateTrumpResult.ExecutionTime} ms");
    }

    private static async Task Insert()
    {
      var client = CreateClient();
      for (int i = 0; i < 10000; i++)
      {
        var response = await client.AddAsync(new Tweet(i, $"tweet{i} content", $"someSource #tag{i}", i, 10, DateTime.Now));
        if (!response.Success)
        {
          Console.WriteLine(response);
        }
      }
    }

    private static async Task PredefinedQueries()
    {
      try
      {
        IDatabaseInteractor dbInteractor = new MongoDatabaseInteractor("/mongo", "clccontainer1.cloudapp.net", 27017, "<username>", "<password>");

        var tweets = await dbInteractor.GetQueryResultAsync(new GetTweetsWithHashtagQuery("#tag1"));
        tweets.Result?.Take(10).ToList().ForEach(Console.WriteLine);

        var avgFollowers = dbInteractor.GetQueryResultAsync(new GetAverageFollowersQuery());

        Console.WriteLine($"AVG Followers: {avgFollowers}");

      }
      catch (Exception ex)
      {
        Console.WriteLine($"General Exception : {ex.Message}.");
      }
    }
  }
}
