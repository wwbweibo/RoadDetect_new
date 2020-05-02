using System.Threading;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.MessageHandler;

namespace Wwbweibo.CrackDetect.MasterService.Services
{
    public class MessageListenerService
    {
        public IKafkaService kafkaService { get; set; }
        public CrackDbContext dbContext { get; set; }

        public TaskControlMessageHandler TaskControlMessageHandler { get; set; }
        public TaskItemDataMessageHandler TaskItemDataMessageHandler { get; set; }
        public CrackCalcMessageHandler CrackCalcMessageHandler { get; set; }

        public void StartMessageListener(MessageTopicEnum[] topics)
        {
            foreach (var topic in topics)
            {
                switch (topic)
                {
                    case MessageTopicEnum.TaskControl:
                        Task.Run(() =>
                            kafkaService.ListenMessage(new string[] { ((int)topic).ToString() },
                                ServiceType.MasterService.ToString(),
                                new CancellationTokenSource(), TaskControlMessageHandler.HandelMessage));
                        break;
                    case MessageTopicEnum.TaskItemData:
                        Task.Run(() =>
                       kafkaService.ListenMessage(new string[] { ((int)topic).ToString() },
                           ServiceType.MasterService.ToString(),
                           new CancellationTokenSource(), TaskItemDataMessageHandler.HandelMessage));
                        break;
                    case MessageTopicEnum.TaskCalc:
                        Task.Run(() =>
                        kafkaService.ListenMessage(new string[] { ((int)topic).ToString() },
                            ServiceType.MasterService.ToString(),
                            new CancellationTokenSource(), CrackCalcMessageHandler.HandelMessage));
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
