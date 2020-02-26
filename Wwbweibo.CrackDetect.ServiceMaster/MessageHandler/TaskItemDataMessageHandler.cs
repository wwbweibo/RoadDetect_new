using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;
using Google.Protobuf;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class TaskItemDataMessageHandler:BaseMessageHandler
    {
        public TaskItemDataMessageHandler(CrackDbContext dbContext) : base(dbContext)
        {

        }

        public override void HandelMessage(object sender, string message)
        {
            TaskModel taskModel = new TaskModel();
            taskModel.MergeFrom(message.DecodeBase64String());
            dbContext.TaskItems.Add(new TaskItem()
            {
                Area = 0,
                Data = taskModel.SubTaskData,
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
