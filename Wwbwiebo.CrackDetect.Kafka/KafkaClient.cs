using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Tools;

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
                }
                catch (ProduceException<Null, string> e)
                {
                    Logger.Error($"Delivery failed: {e.Error.Reason}", e);
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
                            Logger.Info($"kafka message arrive: {cr.Value}");
                            OnMessage?.Invoke(this, cr.Value);
                        }
                        catch (ConsumeException e)
                        {
                            Logger.Error($"Error occured: {e.Error.Reason}", e);
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
