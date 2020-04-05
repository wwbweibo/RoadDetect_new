using System;
using System.Collections.Generic;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class TaskService
    {
        public CrackDbContext DbContext { get; set; }

        public List<TaskViewModel> GetAllTasks()
        {
            List<TaskViewModel> tasks = new List<TaskViewModel>();
            foreach (var task in DbContext.Tasks.ToList())
            {
                var taskViewModel = new TaskViewModel();
                taskViewModel.StartTime = task.CreateTime;
                taskViewModel.EndTime = task.EndTime;
                taskViewModel.MajorTaskId = task.Id;
                var firstItem = DbContext.TaskItems.First(p => p.MajorTaskId == task.Id);
                if (firstItem != null)
                {
                    taskViewModel.Position = new Position()
                        {Longitude = firstItem.Longitude, Latitude = firstItem.Latitude};
                    taskViewModel.ImageData = firstItem.Data;
                }

                tasks.Add(taskViewModel);
            }

            return tasks;
        }

        public TaskViewModel GetTaskDetails(Guid taskId)
        {
            var taskViewModel = new TaskViewModel();
            var dbModel = DbContext.Tasks.First(p => p.Id == taskId);
            if (dbModel != null)
            {
                taskViewModel.StartTime = dbModel.CreateTime;
                taskViewModel.EndTime = dbModel.EndTime;
                taskViewModel.MajorTaskId = dbModel.Id;
                taskViewModel.TaskItemList = DbContext.TaskItems.Where(p => p.MajorTaskId == taskId).Select(p => new TaskItemViewModel()
                {
                    MajorTaskId = taskId,
                    OriginImageData = p.Data,
                    IsCrack = p.IsCrack,
                    Area =  p.Area,
                    Position = new Position() { Longitude = p.Longitude, Latitude = p.Latitude},
                    TaskItemId = p.Id
                }).ToList();
            }

            return taskViewModel;
        }

    }
}
