using Google.Protobuf;
using System;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class TaskItemDataMessageHandler : BaseMessageHandler
    {
        public TaskItemDataMessageHandler(CrackDbContext dbContext) : base(dbContext)
        {

        }

        public override void HandelMessage(object sender, string message)
        {
            TaskItemModel taskModel = new TaskItemModel();
            taskModel.MergeFrom(message.DecodeBase64String());
            lock (CrackDbContext.obj)
            {
                dbContext.TaskItems.Add(new TaskItem()
                {
                    Area = 0,
                    Id = Guid.Parse(taskModel.SubTaskId),
                    IsCrack = false,
                    Latitude = taskModel.Position.Latitude,
                    Longitude = taskModel.Position.Longitude,
                    MajorTaskId = Guid.Parse(taskModel.MajorTaskId),
                    MarkedData = ""
                });
                dbContext.SaveChanges();
            }

        }
    }
}
