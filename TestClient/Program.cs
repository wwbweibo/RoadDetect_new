using System;
using System.IO;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var imagePath = @"D:\OneDrive\Pictures\47526064_p0.png.jpg";
            var image = File.Open(imagePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[image.Length];
            var majorTaskId = Guid.NewGuid();
            var taskStartTime = new DateTime();
            image.Read(buffer, 0, (int)image.Length);
            var b64Data = buffer.EncodeBytesToBase64String();
            var taskModel = new TaskModel()
            {
                MajorTaskId = majorTaskId.ToString(),
                Latitude = "",
                Longitude = "",
                ResultData = "",
                ResultId = "",
                SubTaskData = b64Data,
                SubTaskId = Guid.NewGuid().ToString(),
                SubTaskTime = DateTime.Now.ToString(),
                TaksStartTime = taskStartTime.ToString(),
            };

            RedisClient redisClient = new RedisClient("ali.wwbweibo.me", "6793");
            redisClient.HSet(majorTaskId.ToString(), taskModel.SubTaskId, taskModel.SubTaskData);
            ZookeeperClient zkClient =
                ZookeeperClient.InitClientConnection(new string[] { "ali.wwbweibo.me" }, new string[] { "2181" });
            zkClient.CreateTask(TaskType.CrackCalc.ToString(), majorTaskId.ToString() + "/" + taskModel.SubTaskId);
            KafkaClient client = new KafkaClient("ali.wwbweibo.me", "9092");
            client.SendMessageAsync(TaskType.CrackCalc.ToString(), taskModel.ToString()).GetAwaiter().GetResult();
        }
    }
}
