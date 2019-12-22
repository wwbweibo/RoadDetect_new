using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Wwbweibo.CrackDetect.Tools.String;

namespace Wwbwiebo.CrackDetect.Kafka
{
    public class KafkaClient
    {
        #region Feild
        private string server;
        private string port;
        private string groupId;
        IEnumerable<string> topics;
        private ConsumerConfig config;
        private ConsumerBuilder<Ignore, string> builder;
        #endregion
        #region Attr
        public string Server { get { return server; } }
        public string Port { get { return port; } }
        public string GroupId { get { return groupId; } }
        public List<string> Topics { get { return topics as List<string>; } }
        public ConsumerConfig Config { get { return config; } }
        public ConsumerBuilder<Ignore, string> Builder { get { return builder; } }
        #endregion 

        public KafkaClient(string server, string port, string groupId, IEnumerable<string> topics)
        {
            this.server = server;
            this.port = port;
            this.topics = topics;
            this.groupId = groupId;
        }

        public bool Connect()
        {
            if(!server.IsNullOrEmpty() && !port.IsNullOrEmpty() && (topics as IList<string>).Count > 0 && config ==null)
            {
                config = new ConsumerConfig()
                {
                    BootstrapServers = $"{server}:{port}",
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                };
                builder = new ConsumerBuilder<Ignore, string>(config);
                builder.Build().Assignment.;
                return true;
            }
            return false;
        }
    }
}
