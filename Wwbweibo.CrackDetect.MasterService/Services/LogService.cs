﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.MasterService.Services
{
    public class LogService
    {
        public IRedisClient redisClient { get; set; }

        public Task<LogModel[]> GetAllLogMessages()
        {
            var t = Task.Run(() =>
            {
                var data = new List<string>();
                foreach (int serviceType in Enum.GetValues(typeof(ServiceType)))
                {
                    foreach (int logLevel in Enum.GetValues(typeof(LogLevel)))
                    {
                        data.AddRange(redisClient.LPop(serviceType.ToString() + logLevel.ToString()));
                    }
                }

                return data.Select(p => LogModel.Parser.ParseFrom(Encoding.UTF8.GetBytes(p))).OrderBy(p => p.LogTime).ToArray();
            });

            return t;
        }

        public LogModel[] GetSpecifyLog(int logLevel, int serviceType)
        {
            return redisClient.LPop(serviceType.ToString() + logLevel.ToString()).Select(p => LogModel.Parser.ParseFrom(Encoding.UTF8.GetBytes(p))).ToArray();
        }
    }
}
