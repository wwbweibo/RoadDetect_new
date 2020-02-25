using Google.Protobuf;
using System;
using System.IO;
using System.IO.Compression;
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
            KafkaClient client = new KafkaClient("ali.wwbweibo.me", "9092");
            var majorTaskId = Guid.NewGuid();
            // 首先开始一个任务
            var taskControlMessage = new TaskControlModel(){Id = majorTaskId.ToString(), Action = "START", Time = DateTime.Now.ToString()};

            client.SendMessageAsync(((int)MessageTopicEnum.TaskControl).ToString(), 
                    taskControlMessage.ToB64Data()).GetAwaiter()
                .GetResult();
            // 创建一个任务的项目
            var imagePath = @"C:\Users\wwbwe\OneDrive\Pictures\47526064_p0.png.jpg";
            var image = File.Open(imagePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[image.Length];
            var taskStartTime = new DateTime();
            image.Read(buffer, 0, (int)image.Length);
            var b64Data = buffer.EncodeBytesToBase64String();
            var taskModel = new TaskModel()
            {
                MajorTaskId = majorTaskId.ToString(),
                Position = new Position() { Latitude = 123.31, Longitude = 232.4232},
                ResultData = "",
                ResultId = "",
                SubTaskData = "",
                SubTaskId = Guid.NewGuid().ToString(),
                SubTaskTime = DateTime.Now.ToString(),
            };

            RedisClient redisClient = new RedisClient("ali.wwbweibo.me", "6793");
            ZookeeperClient zkClient =
                ZookeeperClient.InitClientConnection(new string[] { "ali.wwbweibo.me" }, new string[] { "2181" });
            zkClient.CreateTask(TaskType.CrackCalc.ToString(), majorTaskId.ToString() + "/" + taskModel.SubTaskId);

            client.SendMessageAsync( ((int)MessageTopicEnum.TaskItemData).ToString(), taskModel.ToB64Data()).GetAwaiter().GetResult();
            

            // 现在关闭任务

            taskControlMessage.Action = "STOP";
            taskControlMessage.Time = DateTime.Now.ToString();
            client.SendMessageAsync(((int) MessageTopicEnum.TaskControl).ToString(), taskControlMessage.ToB64Data())
                .GetAwaiter().GetResult();
        }
    }
}
