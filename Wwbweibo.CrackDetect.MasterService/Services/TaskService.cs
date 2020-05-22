using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;
using Wwbweibo.CrackDetect.ServiceMaster.Models;

namespace Wwbweibo.CrackDetect.MasterService.Services
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
        public async Task<List<TaskViewModel>> GetAllTasks()
        {
            var t = Task.Run(() =>
            {
                List<TaskViewModel> tasks = new List<TaskViewModel>();
                lock (DbContext.obj)
                {
                    foreach (var task in DbContext.Tasks.ToList())
                    {
                        var taskViewModel = new TaskViewModel();
                        taskViewModel.StartTime = task.CreateTime;
                        taskViewModel.EndTime = task.EndTime;
                        taskViewModel.MajorTaskId = task.Id;
                        var firstItem = DbContext.TaskItems.FirstOrDefault(p => p.MajorTaskId == task.Id);
                        if (firstItem != null && firstItem.Id != Guid.Empty)
                        {
                            taskViewModel.Position = new Position()
                                { Longitude = firstItem.Longitude, Latitude = firstItem.Latitude };
                            taskViewModel.ImageData = firstItem.Data;
                        }

                        tasks.Add(taskViewModel);
                    }
                }

                return tasks;
            });

            return await t;
        }

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<TaskViewModel> GetTaskDetails(Guid taskId)
        {
            var t = Task.Run( () =>
            {
                var taskViewModel = new TaskViewModel();
                lock (DbContext.obj)
                {
                    var dbModel = DbContext.Tasks.FirstOrDefault(p => p.Id == taskId);
                    if (dbModel != null || dbModel.Id!=Guid.Empty)
                    {
                        taskViewModel.StartTime = dbModel.CreateTime;
                        taskViewModel.EndTime = dbModel.EndTime;
                        taskViewModel.MajorTaskId = dbModel.Id;
                        taskViewModel.TaskItemList = DbContext.TaskItems.Where(p => p.MajorTaskId == taskId).Select(p =>
                            new TaskItemViewModel()
                            {
                                MajorTaskId = taskId,
                                OriginImageData = p.Data,
                                IsCrack = p.IsCrack,
                                Area = p.Area,
                                Position = new Position() {Longitude = p.Longitude, Latitude = p.Latitude},
                                TaskItemId = p.Id,
                                MarkedImageData = p.MarkedData
                            }).ToList();
                    }
                }

                return taskViewModel;
            });

            return await t;
        }

        /// <summary>
        /// 获取所有待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> ListAllTodoTask()
        {
            var result = new Dictionary<string, int>();

            var tasks = await ZkClient.ListChildren(ConstData.TodoTaskPath);
            if (tasks != null)
            {
                foreach(var item in tasks)
                {
                    var count = await ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + item.Item1);
                    result.Add(item.Item1, count.Count);
                }
            }

            return result;
        }

        public async Task<List<string>> GetTodoTask(string id)
        {
            return (await ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + id))?.Select(p => p.Item1).ToList();
        }

        /// <summary>
        /// 获取没有在处理的待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>> ListUndoingTask()
        {
            var data = await ListAllTodoTask();
            var result = new Dictionary<string, List<string>>();
            foreach (var keyValuePair in data)
            {
                var inProgressTask = await ZkClient.ListChildren(ConstData.InProgressPath + "/" + keyValuePair.Key);
                var inProgress = new List<string>();
                if(inProgressTask != null && inProgressTask.Count > 0)
                {
                    inProgress = inProgressTask.Select(p => p.Item1).ToList() ;
                }
                var todoTask = await ZkClient.ListChildren(ConstData.TodoTaskPath + "/" + keyValuePair.Key);
                var todo = new List<string>();
                if(todoTask.Count > 0)
                {
                    todo = todoTask.Select(p => p.Item1).ToList();
                }
                if(todo.Count > 0)
                {
                    result.Add(keyValuePair.Key, todo.Except(inProgress).ToList());
                }
                
            }
            return result;
        }
        /// <summary>
        /// 重新分发待办任务
        /// </summary>
        /// <param name="taskType"></param>
        public async Task DistributeTask(TaskType taskType)
        {
            var taskList = await ListUndoingTask();
            foreach (var tasks in taskList)
            {
                foreach (var task in tasks.Value)
                {
                    KafkaService.SendMessageAsync((int)MessageTopicEnum.TaskItemData + "", task).GetAwaiter().GetResult();
                }
            }
        }

       public async Task DistributeTask(string majorTaskId)
        {
            var taskList = await ListUndoingTask();
            if (taskList.ContainsKey(majorTaskId))
            {
                foreach (var task in taskList[majorTaskId])
                {
                    await KafkaService.SendMessageAsync((int)MessageTopicEnum.TaskItemData + "", new TaskItemModel() { 
                        MajorTaskId = majorTaskId, SubTaskId = task
                    }.ToB64Data());
                }
             
            }
            else
            {
                throw new ApplicationException("该任务下没有待处理的数据");
            }
        }

    }
}
