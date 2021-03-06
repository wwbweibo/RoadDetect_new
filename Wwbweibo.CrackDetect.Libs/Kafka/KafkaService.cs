﻿using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.Libs.Kafka
{
    public class KafkaService : IKafkaService
    {
        public string Server { get; private set; }
        public string Port { get; private set; }

        public KafkaService(string server, string port)
        {
            this.Server = server;
            this.Port = port;
        }

        public async Task<bool> SendMessageAsync(string topic, string message)
        {
            var config = new ProducerConfig { BootstrapServers = $"{Server}:{Port}" };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var msg = new KafkaMessageModel() { data = message, CreateTime = Tools.Tools.GetTimeStamp() };
                    var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = Tools.Tools.Parse2Json(msg) });
                }
                catch (ProduceException<Null, string> e)
                {
                    return false;
                }
            }
            return true;
        }

        public void ListenMessage(string[] topics, string groupId, CancellationTokenSource cts, Action<object, string> onMessageCallback)
        {
            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = $"{Server}:{Port}",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topics);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            if (cr.IsPartitionEOF)
                            {
                                continue;
                            }

                            var message = JsonConvert.DeserializeObject<KafkaMessageModel>(cr.Value);
                            onMessageCallback.Invoke(this, message.data);
                        }
                        catch (ConsumeException e)
                        {
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}
