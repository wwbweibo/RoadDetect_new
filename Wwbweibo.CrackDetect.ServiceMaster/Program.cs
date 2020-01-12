using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wwbweibo.CrackDetect.Kafka;
using Wwbweibo.CrackDetect.ServiceMaster.Models;
using Wwbweibo.CrackDetect.Zookeeper;

namespace Wwbweibo.CrackDetect.ServiceMaster
{
    public class Program
    {
        private static KafkaClient kafkaClient;
        private static ZookeeperClient zkClient;
        private static readonly Guid ServiceId = Guid.NewGuid();

        public static void Main(string[] args)
        {
            kafkaClient = new KafkaClient("ali.wwbweibo.me", "6379");
            zkClient = ZookeeperClient.InitClientConnection(new string[] { "ali.wwbweibo.me" }, new string[] { "2181" });
            RegisterSelf();
            CreateHostBuilder(args).Build().Run();
        }

        public static KafkaClient GetKafkaClient()
        {
            return kafkaClient;
        }

        public static ZookeeperClient GetZookeeperClient()
        {
            return zkClient;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// 向Zookeeper注册本服务
        /// </summary>
        public static void RegisterSelf()
        {
            zkClient.RegisterService(ConstData.ServiceMasterServiceName, ServiceId);
        }
    }
}
