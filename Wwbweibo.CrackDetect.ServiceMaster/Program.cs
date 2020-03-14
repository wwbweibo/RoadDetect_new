using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster
{
    public class Program
    {
        private static readonly Guid ServiceId = Guid.NewGuid();

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // 启动消息监听
            host.Services.GetService<MessageListenerService>()
                .StartMessageListener(new[] { MessageTopicEnum.TaskItemData, MessageTopicEnum.TaskControl, MessageTopicEnum.TaskCalc });
            // 注册服务
            host.Services.GetService<ZookeeperClient>()
                .RegisterService((int) ServiceType.MasterService + "", ServiceId);

            host.StartAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
