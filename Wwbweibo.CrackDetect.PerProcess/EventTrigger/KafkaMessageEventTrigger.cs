using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Kafka;

namespace Wwbweibo.CrackDetect.PerProcess.EventTrigger
{
    public class KafkaMessageEventTrigger
    {
        private static KafkaClient client;

        public static void OnMessageTrigger(string server, string port, CancellationTokenSource cts)
        {
            Task.Run(() =>
            {
                if (client == null)
                {
                    client = new KafkaClient(server, port);
                    client.OnMessage += (sender, message) =>
                    {
                        // todo: here to get data and call worker to process
                    };
                    client.ListenMessage(new string[] { "PerProcessTopic"}, "PerProcessGroup", cts);
                }
            });
        }

        private static void Worker(int id, string b64Image)
        {

        }
    }
}
