using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;

namespace Wwbweibo.CrackDetect.Libs.Kafka
{
    public interface IKafkaService
    {
        Task<bool> SendMessageAsync(string topic, string message);
        void ListenMessage(string[] topics, string groupId, CancellationTokenSource cts, Action<object, string> onMessageCallback);
    }
}
