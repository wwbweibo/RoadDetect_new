using System;
using Wwbweibo.CrackDetect.Kafka;
using System.IO;
using Wwbweibo.CrackDetect.Tools.String;

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
            KafkaClient client = new KafkaClient("ali.wwbweibo.me","9092");
            client.SendMessageAsync("preprocess", Guid.NewGuid().ToString()).GetAwaiter().GetResult();
        }
    }
}
