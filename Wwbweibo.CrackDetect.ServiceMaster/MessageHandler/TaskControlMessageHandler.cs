using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Wwbweibo.CrackDetect.Libs.MySql;
using DbTask = Wwbweibo.CrackDetect.Libs.MySql.Entity.Task;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class TaskControlMessageHandler:BaseMessageHandler
    {
        public TaskControlMessageHandler(CrackDbContext dbContext) : base(dbContext)
        {

        }
        public override void HandelMessage(object sender, string message)
        {
            TaskControlModel taskControlModel = new TaskControlModel();
            taskControlModel.MergeFrom(message.DecodeBase64String());
            if (taskControlModel.Action == "START")
            {
                StartTaskHandler(taskControlModel);
            }else if (taskControlModel.Action == "STOP")
            {
                StopTaskHandler(taskControlModel);
            }
        }

        private void StartTaskHandler(TaskControlModel taskControlModel)
        {
            dbContext.Tasks.Add(new DbTask()
            {
                Id = taskControlModel.Id,
                CrackTotalCount = 0,
                DataTotalCount = 0,
                CreateTime = DateTime.Parse(taskControlModel.Time)
            });
            dbContext.SaveChanges();
        }

        private void StopTaskHandler(TaskControlModel taskControlModel)
        {
            var task = dbContext.Tasks.FirstOrDefault(p => p.Id == taskControlModel.Id);
            task.EndTime = DateTime.Now;
            dbContext.SaveChanges();
        }
    }
}
