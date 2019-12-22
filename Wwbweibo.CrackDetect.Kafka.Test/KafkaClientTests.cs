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
        [TestMethod()]
        public void ConnectTest()
        {
            string server = "ali.wwbweibo.me";
            string port = "9092";
            var topics = new List<string>(){ "test"};
            var client = new KafkaClient(server, port, "test", topics);
            Assert.IsTrue(client.Connect());
        }
    }
}