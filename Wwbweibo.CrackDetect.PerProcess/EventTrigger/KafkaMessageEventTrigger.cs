using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Kafka;

namespace Wwbweibo.CrackDetect.PerProcess.EventTrigger
{
    public class KafkaMessageEventTrigger
    {
        private KafkaClient client;

        public static void OnMessageTrigger()
        {
        }

        private static void Worker(int id, string b64Image)
        {

        }
    }
}
