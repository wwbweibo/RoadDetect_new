using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wwbwiebo.CrackDetect.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wwbwiebo.CrackDetect.Kafka.Tests
{
    [TestClass()]
    public class KafkaClientTests
    {
        [TestMethod]
        public void SendMessageTest()
        {
            KafkaClient client = new KafkaClient("47.98.170.195", "9092");
            Assert.IsTrue(client.SendMessageAsync("test", "testMessage").Result);
        }

        [TestMethod]
        public void ListenMessageTest()
        {

            KafkaClient client = new KafkaClient("localhost", "9092");
            client.OnMessage += (sender, message) => { Assert.IsTrue(true); };
            client.ListenMessage(new string[]{"test"},"test" );
            
            Assert.Fail();
        }
    }
}