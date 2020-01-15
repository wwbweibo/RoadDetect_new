using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Wwbweibo.CrackDetect.Redis;
using Wwbweibo.CrackDetect.ServiceMaster.Models;
using Newtonsoft.Json;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class LogService
    {
        private static RedisClient redisClient;
        public LogService(RedisClient redisClient)
        {
            LogService.redisClient = redisClient;
        }

        public LogModel[] GetAllLogMessages()
        {
            var data = new List<string>();
            foreach (var serviceType in ConstData.ServiceTypes)
            {
                foreach (var logLevel in ConstData.LogLevelList)
                {
                    data.AddRange(redisClient.LPop(serviceType + logLevel));
                }
            }

            return data.Select(JsonConvert.DeserializeObject<LogModel>).ToArray();
        }

        public LogModel[] GetSpecifyLog(string logLevel, string serviceType)
        {
            return redisClient.LPop(serviceType + logLevel).Select(JsonConvert.DeserializeObject<LogModel>).ToArray();
        }
    }
}
