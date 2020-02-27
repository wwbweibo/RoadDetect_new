using Google.Protobuf;
using System;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class CrackCalcMessageHandler : BaseMessageHandler
    {
        public CrackCalcMessageHandler(CrackDbContext dbContext) : base(dbContext)
        {

        }

        public override void HandelMessage(object sender, string message)
        {
            TaskResultModel taskModel = new TaskResultModel();
            taskModel.MergeFrom(message.DecodeBase64String());
            var item = dbContext.TaskItems.FirstOrDefault(p => p.Id == Guid.Parse(taskModel.SubTaskId));
            if (item != null)
            {
                item.Data = redisClient.HGet(taskModel.MajorTaskId, taskModel.SubTaskId);
                item.IsCrack = taskModel.IsCrack;
                item.Area = taskModel.CrackArea;
            }

            dbContext.SaveChanges();
        }
    }
}
