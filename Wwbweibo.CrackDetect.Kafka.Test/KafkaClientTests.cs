using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wwbweibo.CrackDetect.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Wwbweibo.CrackDetect.Kafka.Tests
{
    [TestClass()]
    public class KafkaClientTests
    {
        [TestMethod]
        public void SendMessageTest()
        {
            KafkaClient client = new KafkaClient("ali.wwbweibo.me", "9092");
            Assert.IsTrue(client.SendMessageAsync("test", "testMessage").Result);
        }

        [TestMethod]
        public void ListenMessageTest()
        {
            var cts = new CancellationTokenSource();
            var result = false;
            KafkaClient client = new KafkaClient("ali.wwbweibo.me", "9092");
            client.OnMessage += (sender, message) => {
                cts.Cancel();
                result = true;
            };
            client.ListenMessage(new string[]{"test"},"test",cts);
            // send a message to test the evnet is trigger
            client.SendMessageAsync("test", "test").GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
    }
}