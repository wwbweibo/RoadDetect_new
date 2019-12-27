using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wwbweibo.CrackDetect.PerProcess.EventTrigger;

namespace Wwbweibo.CrackDetect.PerProcess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            KafkaMessageEventTrigger.OnMessageTrigger("ali.wwbweibo.me","9092", new CancellationTokenSource());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
