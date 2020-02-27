using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wwbweibo.CrackDetect.Libs.Kafka
{
    public interface IKafkaService
    {
        Task<bool> SendMessageAsync(string topic, string message);
        void ListenMessage(string[] topics, string groupId, CancellationTokenSource cts, Action<object, string> onMessageCallback);
    }
}
