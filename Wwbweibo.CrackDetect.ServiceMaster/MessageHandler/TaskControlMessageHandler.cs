using Google.Protobuf;
using System;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;
using DbTask = Wwbweibo.CrackDetect.Libs.MySql.Entity.Task;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class TaskControlMessageHandler : BaseMessageHandler
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
            }
            else if (taskControlModel.Action == "STOP")
            {
                StopTaskHandler(taskControlModel);
            }
        }

        private void StartTaskHandler(TaskControlModel taskControlModel)
        {
            lock (CrackDbContext.obj)
            {

                dbContext.Tasks.Add(new DbTask()
                {
                    Id = Guid.Parse(taskControlModel.Id),
                    CrackTotalCount = 0,
                    DataTotalCount = 0,
                    CreateTime = DateTime.Parse(taskControlModel.Time)
                });
                dbContext.SaveChanges();
            }
        }

        private void StopTaskHandler(TaskControlModel taskControlModel)
        {
            lock (CrackDbContext.obj)
            {
                var task = dbContext.Tasks.FirstOrDefault(p => p.Id == Guid.Parse(taskControlModel.Id));
                if (task != null)
                {
                    task.EndTime = DateTime.Now;
                    dbContext.Tasks.Update(task);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
