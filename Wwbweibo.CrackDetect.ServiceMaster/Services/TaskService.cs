using System;
using System.Collections.Generic;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class TaskService
    {
        public CrackDbContext DbContext { get; set; }
        public ZookeeperClient ZkClient { get; set; }
        public IKafkaService KafkaService { get; set; }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
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
                    TaskItemId = p.Id,
                    MarkedImageData =  p.MarkedData
                }).ToList();
            }

            return taskViewModel;
        }

        /// <summary>
        /// 获取所有待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public  Dictionary<string, int> ListAllTodoTask()
        {
            var result = new Dictionary<string, int>();

            var tasks = ZkClient.ListChildren(ConstData.TodoTaskPath).Result;
            if (tasks != null)
            {
                tasks.ForEach(p =>
                {
                    result.Add(p.Item1, ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + p.Item1).Result.Count);
                });
            }

            return result;
        }

        public List<string> GetTodoTask(string id)
        {
            return ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + id).Result?.Select(p => p.Item1).ToList();
        }

        /// <summary>
        /// 获取没有在处理的待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ListUndoingTask()
        {
            var data = ListAllTodoTask();
            var result = new Dictionary<string, List<string>>();
            foreach (var keyValuePair in result)
            {
                var inProgress = ZkClient.ListChildren(ConstData.InProgressPath + "/" + keyValuePair.Key).Result
                    .Select(p => p.Item1);
                var todo = ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + keyValuePair.Key).Result
                    .Select(p => p.Item1);
                result.Add(keyValuePair.Key, todo.Except(inProgress).ToList());
            }
            return result;
        }
        /// <summary>
        /// 重新分发待办任务
        /// </summary>
        /// <param name="taskType"></param>
        public void DistributeTask(TaskType taskType)
        {
            var taskList = ListUndoingTask();
            foreach (var tasks in taskList)
            {
                foreach (var task in tasks.Value)
                {
                    KafkaService.SendMessageAsync((int)MessageTopicEnum.TaskItemData + "", task).GetAwaiter().GetResult();
                }
            }
        }

    }
}
