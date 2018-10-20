// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Microsoft.Extensions.Caching.SQLite
{
    internal static class SqlParameterCollectionExtensions
    {
        // For all values where the length is less than the below value, try setting the size of the
        // parameter for better performance.
        public const int DefaultValueColumnWidth = 8000;

        // Maximum size of a primary key column is 900 bytes (898 bytes from the key + 2 additional bytes required by
        // the Sql Server).
        public const int CacheItemIdColumnWidth = 449;

        public static SqliteParameterCollection AddCacheItemId(this SqliteParameterCollection parameters, string value)
        {
            return parameters.AddWithValue(Columns.Names.CacheItemId, SqliteType.Text, CacheItemIdColumnWidth, value);
        }

        public static SqliteParameterCollection AddCacheItemValue(this SqliteParameterCollection parameters, byte[] value)
        {
            if (value != null && value.Length < DefaultValueColumnWidth)
            {
                return parameters.AddWithValue(
                    Columns.Names.CacheItemValue,
                    SqliteType.Blob,
                    DefaultValueColumnWidth,
                    value);
            }
            else
            {
                // do not mention the size
                return parameters.AddWithValue(Columns.Names.CacheItemValue, SqliteType.Blob, value);
            }
        }

        public static SqliteParameterCollection AddSlidingExpirationInSeconds(
            this SqliteParameterCollection parameters,
            TimeSpan? value)
        {
            if (value.HasValue)
            {
                return parameters.AddWithValue(
                    Columns.Names.SlidingExpirationInSeconds, SqliteType.Integer, value.Value.TotalSeconds);
            }
            else
            {
                return parameters.AddWithValue(Columns.Names.SlidingExpirationInSeconds, SqliteType.Integer, DBNull.Value);
            }
        }

        public static SqliteParameterCollection AddAbsoluteExpiration(
            this SqliteParameterCollection parameters,
            DateTimeOffset? utcTime)
        {
            if (utcTime.HasValue)
            {
                return parameters.AddWithValue(
                    Columns.Names.AbsoluteExpiration, SqliteType.Integer, utcTime.Value.ToUnixTimeMilliseconds());
            }
            else
            {
                return parameters.AddWithValue(
                    Columns.Names.AbsoluteExpiration, SqliteType.Integer, DBNull.Value);
            }
        }

        public static SqliteParameterCollection AddWithValue(
            this SqliteParameterCollection parameters,
            string parameterName,
            SqliteType dbType,
            object value)
        {
            var parameter = new SqliteParameter(parameterName, dbType);
            parameter.Value = value;
            parameters.Add(parameter);
            parameter.ResetSqliteType();
            return parameters;
        }

        public static SqliteParameterCollection AddWithValue(
            this SqliteParameterCollection parameters,
            string parameterName,
            SqliteType dbType,
            int size,
            object value)
        {
            var parameter = new SqliteParameter(parameterName, dbType, size);
            parameter.Value = value;
            parameters.Add(parameter);
            parameter.ResetSqliteType();
            return parameters;
        }
    }
}
