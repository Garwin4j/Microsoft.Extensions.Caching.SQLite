// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Internal;


namespace Microsoft.Extensions.Caching.SQLite
{
    internal class SQLiteDatabaseOperations : DatabaseOperations
    {
        public SQLiteDatabaseOperations(
            string connectionString, /*string schemaName,*/ string tableName, ISystemClock systemClock)
            : base(connectionString,/* schemaName,*/ tableName, systemClock)
        {
        }

        protected override byte[] GetCacheItem(string key, bool includeValue)
        {
            var utcNow = SystemClock.UtcNow;

            string query;
            if (includeValue)
            {
                query = SqlQueries.GetCacheItem;
            }
            else
            {
                query = SqlQueries.GetCacheItemWithoutValue;
            }

            byte[] value = null;
            TimeSpan? slidingExpiration = null;
            DateTimeOffset? absoluteExpiration = null;
            DateTimeOffset expirationTime;
            using (var connection = new SqliteConnection(ConnectionString))
            {
                var command = new SqliteCommand(query, connection);
                command.Parameters
                    .AddCacheItemId(key)
                    .AddWithValue("UtcNow", SqliteType.Integer, utcNow.ToUnixTimeMilliseconds());

                connection.Open();

                var reader = command.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.SingleResult);

                if (reader.Read())
                {
                    var id = reader.GetString(Columns.Indexes.CacheItemIdIndex);

                    expirationTime = DateTimeOffset.Parse(reader[Columns.Indexes.ExpiresAtTimeIndex].ToString());

                    if (!reader.IsDBNull(Columns.Indexes.SlidingExpirationInSecondsIndex))
                    {
                        slidingExpiration = TimeSpan.FromSeconds(
                            reader.GetInt64(Columns.Indexes.SlidingExpirationInSecondsIndex));
                    }

                    if (!reader.IsDBNull(Columns.Indexes.AbsoluteExpirationIndex))
                    {
                        absoluteExpiration = DateTimeOffset.Parse(
                            reader[Columns.Indexes.AbsoluteExpirationIndex].ToString());
                    }

                    if (includeValue)
                    {
                        value = (byte[])reader[Columns.Indexes.CacheItemValueIndex];
                    }
                }
                else
                {
                    return null;
                }
            }

            return value;
        }

        protected override async Task<byte[]> GetCacheItemAsync(string key, bool includeValue, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            var utcNow = SystemClock.UtcNow;

            string query;
            if (includeValue)
            {
                query = SqlQueries.GetCacheItem;
            }
            else
            {
                query = SqlQueries.GetCacheItemWithoutValue;
            }

            byte[] value = null;
            TimeSpan? slidingExpiration = null;
            DateTimeOffset? absoluteExpiration = null;
            DateTimeOffset expirationTime;
            using (var connection = new SqliteConnection(ConnectionString))
            {
                var command = new SqliteCommand(SqlQueries.GetCacheItem, connection);
                command.Parameters
                    .AddCacheItemId(key)
                    .AddWithValue("UtcNow", SqliteType.Integer, utcNow.ToUnixTimeMilliseconds());

                await connection.OpenAsync(token);

                var reader = await command.ExecuteReaderAsync(
                    CommandBehavior.SingleRow | CommandBehavior.SingleResult,
                    token);

                if (await reader.ReadAsync(token))
                {
                    var id = reader.GetString(Columns.Indexes.CacheItemIdIndex);
                    expirationTime= DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(reader[Columns.Indexes.ExpiresAtTimeIndex].ToString()));
                  //  expirationTime = DateTimeOffset.Parse(reader[Columns.Indexes.ExpiresAtTimeIndex].ToString());

                    if (!await reader.IsDBNullAsync(Columns.Indexes.SlidingExpirationInSecondsIndex, token))
                    {
                        slidingExpiration = TimeSpan.FromSeconds(
                            Convert.ToInt64(reader[Columns.Indexes.SlidingExpirationInSecondsIndex].ToString()));
                    }

                    if (!await reader.IsDBNullAsync(Columns.Indexes.AbsoluteExpirationIndex, token))
                    {
                        absoluteExpiration = DateTimeOffset.Parse(
                            reader[Columns.Indexes.AbsoluteExpirationIndex].ToString());
                    }

                    if (includeValue)
                    {
                        value = (byte[])reader[Columns.Indexes.CacheItemValueIndex];
                    }
                }
                else
                {
                    return null;
                }
            }

            return value;
        }

        public override void SetCacheItem(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var utcNow = SystemClock.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            ValidateOptions(options.SlidingExpiration, absoluteExpiration);

            using (var connection = new SqliteConnection(ConnectionString))
            {

                /*

        "SET @ExpiresAtTime = " +
       "(CASE " +
               "WHEN (@SlidingExpirationInSeconds IS NUll) " +
               "THEN @AbsoluteExpiration " +
               "ELSE " +
               "DATEADD(SECOND, Convert(bigint, @SlidingExpirationInSeconds), @UtcNow) " +
       "END);" +

            */

                var upsertCommand = new SqliteCommand(SqlQueries.SetCacheItem, connection);
                upsertCommand.Parameters
                    .AddCacheItemId(key)
                    .AddCacheItemValue(value)
                    .AddSlidingExpirationInSeconds(options.SlidingExpiration)
                    .AddAbsoluteExpirationSQLite(absoluteExpiration)
                    //.AddWithValue("UtcNow", SqliteType.Integer, utcNow.ToUnixTimeMilliseconds());
                 .AddExpiresAtTimeSQLite(options.SlidingExpiration == null ? absoluteExpiration.Value : utcNow.Add(options.SlidingExpiration.Value));

                connection.Open();

                try
                {
                    upsertCommand.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    if (IsDuplicateKeyException(ex))
                    {
                        // There is a possibility that multiple requests can try to add the same item to the cache, in
                        // which case we receive a 'duplicate key' exception on the primary key column.
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public override async Task SetCacheItemAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            var utcNow = SystemClock.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(utcNow, options);
            ValidateOptions(options.SlidingExpiration, absoluteExpiration);

            using (var connection = new SqliteConnection(ConnectionString))
            {
                var upsertCommand = new SqliteCommand(SqlQueries.SetCacheItem, connection);
                upsertCommand.Parameters
                    .AddCacheItemId(key)
                    .AddCacheItemValue(value)
                    .AddSlidingExpirationInSeconds(options.SlidingExpiration)
                    .AddAbsoluteExpirationSQLite(absoluteExpiration);
                  //  .AddWithValue("UtcNow", SqliteType.Integer, utcNow.ToUnixTimeMilliseconds());

                upsertCommand = new SqliteCommand("DELETE FROM TestCache  WHERE Id = @Id; ", connection);


                /*
                 
             "SET @ExpiresAtTime = " +
            "(CASE " +
                    "WHEN (@SlidingExpirationInSeconds IS NUll) " +
                    "THEN @AbsoluteExpiration " +
                    "ELSE " +
                    "DATEADD(SECOND, Convert(bigint, @SlidingExpirationInSeconds), @UtcNow) " +
            "END);" +
                 
                 */

                

                upsertCommand = new SqliteCommand(
                               "INSERT INTO TestCache " +
              "(Id, Value, ExpiresAtTime, SlidingExpirationInSeconds, AbsoluteExpiration) " +
              "VALUES (@Id, @Value, @ExpiresAtTime, @SlidingExpirationInSeconds, @AbsoluteExpiration);", connection);

                upsertCommand.Parameters
                    .AddCacheItemId(key)
                    .AddCacheItemValue(value)
                    .AddSlidingExpirationInSeconds(options.SlidingExpiration)
                    .AddAbsoluteExpirationSQLite(absoluteExpiration)
                    .AddExpiresAtTimeSQLite(options.SlidingExpiration==null? absoluteExpiration.Value: utcNow.Add(options.SlidingExpiration.Value));




                await connection.OpenAsync(token);

                try
                {
                    await upsertCommand.ExecuteNonQueryAsync(token);
                }
                catch (SqliteException ex)
                {
                    if (IsDuplicateKeyException(ex))
                    {
                        // There is a possibility that multiple requests can try to add the same item to the cache, in
                        // which case we receive a 'duplicate key' exception on the primary key column.
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public override void DeleteExpiredCacheItems()
        {
            var utcNow = SystemClock.UtcNow;

            using (var connection = new SqliteConnection(ConnectionString))
            {
                var command = new SqliteCommand(SqlQueries.DeleteExpiredCacheItems, connection);
                command.Parameters.AddWithValue("UtcNow", SqliteType.Integer, utcNow.ToUnixTimeMilliseconds());

                connection.Open();

                var effectedRowCount = command.ExecuteNonQuery();
            }
        }
    }
}