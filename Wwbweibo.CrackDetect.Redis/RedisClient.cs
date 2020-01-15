using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StackExchange.Redis;
using Wwbweibo.CrackDetect.Tools.String;

namespace Wwbweibo.CrackDetect.Redis
{
    public class RedisClient
    {
        private static ConnectionMultiplexer connection;
        private static IDatabase database;
        private static IServer redisServer;

        public RedisClient(string server, string port)
        {
            connection = ConnectionMultiplexer.Connect($"{server}:{port}");
            database = connection.GetDatabase();
            redisServer = connection.GetServer(server, port.ToInt());
        }

        public void Set(string key, string value)
        {
            database.StringSet(key, value);
        }

        public string Get(string key)
        {
            return database.StringGet(key);
        }

        public string[] LPop(string key, int start = -1, int end = -1)
        {
            if(start >= 0 && end >= start)
                return database.ListRange(key,start, end).ConvertValuesToStrings();
            return database.ListRange(key).ConvertValuesToStrings();
        }

        public void LPush(string key, string[] values)
        {

            database.ListRightPush(key, values.ConvertStringsToRedisValues());
        }
    }

    public static class RedisValueExtensions
    {
        public static string[] ConvertValuesToStrings(this RedisValue[] values)
        {
            return values.Select(p => p.ToString()).ToArray();
        }

        public static RedisValue[] ConvertStringsToRedisValues(this string[] values)
        {
            return values.Select(p => (RedisValue)p).ToArray();
        }
    }
}
