﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Wwbweibo.CrackDetect.Redis;
using Wwbweibo.CrackDetect.Models;

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
            foreach (int serviceType in Enum.GetValues(typeof(ServiceType)))
            {
                foreach (int logLevel in Enum.GetValues(typeof(LogLevel)))
                {
                    data.AddRange(redisClient.LPop( serviceType.ToString()+ logLevel.ToString()));
                }
            }

            return data.Select(p => LogModel.Parser.ParseFrom(Encoding.UTF8.GetBytes(p))).ToArray();
        }

        public LogModel[] GetSpecifyLog(int logLevel, int serviceType)
        {
            return redisClient.LPop(serviceType.ToString() + logLevel.ToString()).Select(p => LogModel.Parser.ParseFrom(Encoding.UTF8.GetBytes(p))).ToArray();
        }
    }
}
