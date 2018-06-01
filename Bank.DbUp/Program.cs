
using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Bank.DbUp
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configBuilder.Build();

            var connectionString = configuration["connectionString"];

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                            .SqlDatabase(connectionString)
                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                            .LogToConsole()
                            .Build();

            var result = upgrader.PerformUpgrade();

            Console.WriteLine("Success!");
            Console.ReadLine();
        }
    }
}
