using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Wwbweibo.CrackDetect.Tools.String;

namespace Wwbweibo.CrackDetect.Kafka
{
    public class KafkaClient
    {
        public string Server { get; private set; }
        public string Port { get; private set; }

        public delegate void OnMessageHandler(object sender, string message);
        public event OnMessageHandler OnMessage;

        public KafkaClient(string server, string port)
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
                    p.Produce(topic, new Message<Null, string>(){Value = message});
                    var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                    return false;
                }
            }
            return true;
        }

        public void ListenMessage(string[] topics, string groupId)
        {
            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = $"{Server}:{Port}",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            Task.Run(() =>
            {
                using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
                {
                    foreach (var topic in topics)
                    {
                        c.Subscribe(topic);
                    }

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = c.Consume(cts.Token);
                                OnMessage?.Invoke(this, cr.Value);
                            }
                            catch (ConsumeException e)
                            {
                                Console.WriteLine($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        c.Close();
                    }
                }
            });
        }
    }
}
