// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SQLite;
using Microsoft.Extensions.Configuration;
using Dapper;
namespace SqlServerCacheSample
{
    /// <summary>
    /// This sample requires setting up a Microsoft SQL Server based cache database.
    /// 1. Install the .NET Core sql-cache tool globally by installing the dotnet-sql-cache package.
    /// 2. Create a new database in the SQL Server or use an existing one.
    /// 3. Run the command "dotnet sql-cache create <connectionstring> <schemaName> <tableName>" to setup the table and indexes.
    /// 4. Run this sample by doing "dotnet run"
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            RunSampleAsync().Wait();
        }

        public static async Task RunSampleAsync()
        {
            using (SqliteConnection db =
        new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();



                /*
                 
                 USE [CacheSampleDb]
GO


                SET ANSI_NULLS ON
                GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE[dbo].[TestCache]
        (

   [Id] NVARCHAR (449)     COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,

[Value]                      VARBINARY(MAX)    NOT NULL,

[ExpiresAtTime]              DATETIMEOFFSET(7) NOT NULL,

[SlidingExpirationInSeconds] BIGINT NULL,

[AbsoluteExpiration]         DATETIMEOFFSET(7) NULL
);


GO
CREATE NONCLUSTERED INDEX[Index_ExpiresAtTime]
    ON[dbo].[TestCache] ([ExpiresAtTime] ASC);



                 
                 
                 */
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS TestCache (Id NVARCHAR(499) PRIMARY KEY, " +
                    "Value NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }


            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            var key = Guid.NewGuid().ToString();
            var message = "Hello, World!";
            var value = Encoding.UTF8.GetBytes(message);

            Console.WriteLine("Connecting to cache");
            var cache = new SQLiteCache(new SQLiteCacheOptions()
            {
                ConnectionString = configuration["SQLiteConnectionString"],
              //  SchemaName = configuration["SchemaName"],
                TableName = configuration["TableName"]
            });

            Console.WriteLine("Connected");



            Console.WriteLine("Cache item key: {0}", key);
            Console.WriteLine($"Setting value '{message}' in cache");
            await cache.SetAsync(
                key,
                value,
                new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10)));
            Console.WriteLine("Set");

            Console.WriteLine("Getting value from cache");
            value = await cache.GetAsync(key);
            if (value != null)
            {
                Console.WriteLine("Retrieved: " + Encoding.UTF8.GetString(value, 0, value.Length));
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            Console.WriteLine("Refreshing value in cache");
            await cache.RefreshAsync(key);
            Console.WriteLine("Refreshed");

            Console.WriteLine("Removing value from cache");
            await cache.RemoveAsync(key);
            Console.WriteLine("Removed");

            Console.WriteLine("Getting value from cache again");
            value = await cache.GetAsync(key);
            if (value != null)
            {
                Console.WriteLine("Retrieved: " + Encoding.UTF8.GetString(value, 0, value.Length));
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            Console.ReadLine();
        }
    }
}
