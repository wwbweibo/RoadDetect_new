using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster
{
    public class Program
    {
        private static KafkaClient kafkaClient;
        private static ZookeeperClient zkClient;
        private static RedisClient redisClient;
        private static readonly Guid ServiceId = Guid.NewGuid();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static KafkaClient GetKafkaClient()
        {
            return kafkaClient;
        }

        public static RedisClient GetRedisClient()
        {
            return redisClient;
        }

        public static ZookeeperClient GetZookeeperClient()
        {
            return zkClient;
        }

        public static void InitServer(IConfiguration configuration)
        {
            kafkaClient = new KafkaClient("ali.wwbweibo.me", "9092");
            zkClient = ZookeeperClient.InitClientConnection(new string[] { "ali.wwbweibo.me" }, new string[] { "2181" });
            redisClient = new RedisClient(configuration.GetValue<string>("RedisHost"), configuration.GetValue<string>("RedisPort"));
            RegisterSelf();
            
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
            zkClient.RegisterService(ServiceType.MasterService.ToString(), ServiceId);
        }

        /// <summary>
        /// 初始化消息监听
        /// </summary>
        public static void InitMessageListener()
        {
            kafkaClient.ListenMessage(new String []{((int)MessageTopicEnum.TaskControl).ToString(),
                ((int)MessageTopicEnum.TaskItemData).ToString(),
                ((int)MessageTopicEnum.TaskCalc).ToString()
                }, ((int)ServiceType.MasterService).ToString(), new CancellationTokenSource());
            kafkaClient.OnMessage += (sender, message) => { };
        }
    }
}
