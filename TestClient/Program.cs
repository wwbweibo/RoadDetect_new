using System;
using Wwbweibo.CrackDetect.Kafka;
using System.IO;
using Wwbweibo.CrackDetect.Redis;
using Wwbweibo.CrackDetect.Tools.String;
using Wwbweibo.CrackDetect.Zookeeper;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var imagePath = @"D:\OneDrive\Pictures\47526064_p0.png.jpg";
            var image = File.Open(imagePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[image.Length];
            image.Read(buffer, 0, (int)image.Length);
            var b64Data = buffer.EncodeBytesToBase64String();
            var taskId = Guid.NewGuid();
            RedisClient redisClient = new RedisClient("ali.wwbweibo.me", "6793");
            redisClient.Set(taskId.ToString(), b64Data);
            ZookeeperClient zkClient =
                ZookeeperClient.InitClientConnection(new string[] {"ali.wwbweibo.me"}, new string[] {"2181"});
            zkClient.CreateTask("preprocess", taskId.ToString());
            KafkaClient client = new KafkaClient("ali.wwbweibo.me","9092");
            client.SendMessageAsync("preprocess", taskId.ToString()).GetAwaiter().GetResult();
        }
    }
}
