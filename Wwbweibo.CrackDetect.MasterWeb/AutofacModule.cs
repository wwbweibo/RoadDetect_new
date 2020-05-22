using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Module = Autofac.Module;

namespace Wwbweibo.CrackDetect.MasterWeb
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册核心库
            // 注册 Kafka
            builder.Register(c => new KafkaService(c.Resolve<IConfiguration>().GetValue<string>("KafkaHost"),
                    c.Resolve<IConfiguration>().GetValue<string>("KafkaPort")))
                .As<IKafkaService>()
                .SingleInstance();
            // 注册Redis
            builder.Register(c => new RedisClient(
                    c.Resolve<IConfiguration>().GetValue<string>("RedisHost"),
                    c.Resolve<IConfiguration>().GetValue<string>("RedisPort")
                )
            ).AsImplementedInterfaces().SingleInstance();
            // 注册Zookeeper
            builder.Register(c => ZookeeperClient.InitClientConnection( c.Resolve<IConfiguration>().GetValue<string>("ZooKeeperHost").Split(","), 
                c.Resolve<IConfiguration>().GetValue<string>("ZooKeeperPort").Split(","))) 
                .As<ZookeeperClient>().SingleInstance();

            var masterServiceAssembly = Assembly.Load("Wwbweibo.CrackDetect.MasterService");
            // 注册Handler
            builder.RegisterTypes(masterServiceAssembly.GetTypes().Where(p => p.Namespace.EndsWith("Handler")).ToArray())
                .AsSelf().PropertiesAutowired().SingleInstance();

            // 注册Service
            builder.RegisterTypes(masterServiceAssembly.GetTypes().Where(p => p.Namespace.EndsWith("Services")).ToArray())
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();
        }
    }
}
