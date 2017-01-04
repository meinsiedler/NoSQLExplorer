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
      Task.Factory.StartNew(() => CreateSchema());
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
  }
}
