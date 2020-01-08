using System;
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
    }
}
