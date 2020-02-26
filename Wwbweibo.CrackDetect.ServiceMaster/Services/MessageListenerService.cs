using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.MessageHandler;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class MessageListenerService
    {
        private readonly IKafkaService kafkaService;
        private readonly CrackDbContext dbContext;

        public MessageListenerService(IKafkaService kafka, CrackDbContext dbContext)
        {
            this.kafkaService = kafka;
            this.dbContext = dbContext;
        }

        public void StartMessageListener(MessageTopicEnum[] topics)
        {
            foreach (var topic in topics)
            {
                switch (topic)
                {
                    case MessageTopicEnum.TaskControl:
                        Task.Run(() =>
                            kafkaService.ListenMessage(new string[] {((int) topic).ToString()},
                                ServiceType.MasterService.ToString(),
                                new CancellationTokenSource(), new TaskControlMessageHandler(dbContext).HandelMessage));
                        break;
                    case MessageTopicEnum.TaskItemData:
                        Task.Run( ()=>
                        kafkaService.ListenMessage(new string[] { ((int)topic).ToString() },
                            ServiceType.MasterService.ToString(),
                            new CancellationTokenSource(), new TaskItemDataMessageHandler(dbContext).HandelMessage));
                        break;
                    case MessageTopicEnum.TaskCalc:
                        Task.Run(()=>
                        kafkaService.ListenMessage(new string[] { ((int)topic).ToString() },
                            ServiceType.MasterService.ToString(),
                            new CancellationTokenSource(), new CrackCalcMessageHandler(dbContext).HandelMessage));
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
