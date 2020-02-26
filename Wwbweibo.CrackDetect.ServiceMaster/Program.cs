using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster
{
    public class Program
    {
        private static ZookeeperClient zkClient;
        private static RedisClient redisClient;
        private static readonly Guid ServiceId = Guid.NewGuid();

        public static void Main(string[] args)
        {
            var host  = CreateHostBuilder(args).Build();
            // 启动消息监听
            host.Services.GetService<MessageListenerService>()
                .StartMessageListener(new[] { MessageTopicEnum.TaskItemData, MessageTopicEnum.TaskControl, MessageTopicEnum.TaskCalc });
            host.StartAsync();
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
            zkClient = ZookeeperClient.InitClientConnection(new string[] { "ali.wwbweibo.me" }, new string[] { "2181" });
            redisClient = new RedisClient(configuration.GetValue<string>("RedisHost"), configuration.GetValue<string>("RedisPort"));
            RegisterSelf();
            
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
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

    }
}
