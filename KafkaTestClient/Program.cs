using System;
using Wwbweibo.CrackDetect.Kafka;

namespace KafkaTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new KafkaClient("ali.wwbweibo.me", "9092");
            client.SendMessageAsync("test", "test").GetAwaiter().GetResult();
        }
    }
}
