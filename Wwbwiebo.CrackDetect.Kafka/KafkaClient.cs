using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        public void ListenMessage(string[] topics, string groupId, CancellationTokenSource cts)
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
        }
    }
}
