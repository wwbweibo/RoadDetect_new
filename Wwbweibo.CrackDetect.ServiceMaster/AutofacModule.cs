using Autofac;
using Microsoft.Extensions.Configuration;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.ServiceMaster.Services;

namespace Wwbweibo.CrackDetect.ServiceMaster
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new KafkaService(c.Resolve<IConfiguration>().GetValue<string>("KafkaHost"),
                    c.Resolve<IConfiguration>().GetValue<string>("KafkaPort")))
                .As<IKafkaService>()
                .SingleInstance();
            builder.Register(c => new MessageListenerService(c.Resolve<IKafkaService>(), c.Resolve<CrackDbContext>()));
        }
    }
}
