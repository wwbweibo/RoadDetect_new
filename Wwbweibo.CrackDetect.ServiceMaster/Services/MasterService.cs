using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Kafka;
using Wwbweibo.CrackDetect.ServiceMaster.Models;
using Wwbweibo.CrackDetect.Zookeeper;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class MasterService
    {
        private static KafkaClient kafkaClient;
        private static ZookeeperClient zkClient;

        public MasterService()
        {
            kafkaClient = Program.GetKafkaClient();
            zkClient = Program.GetZookeeperClient();
        }

        /// <summary>
        /// 获取所有待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ListAllTodoTask()
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var task in ConstData.TaskTypes)
            {
                result.Add(task, zkClient.ListChildren("/" + task + "/todo"));
            }

            return result;
        }

        public List<string> ListAllTodoTask(string taskType)
        {
            return ListAllTodoTask()[taskType];
        }

        /// <summary>
        /// 获取没有在处理的待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ListUndoingTask()
        {
            var result = ListAllTodoTask();
            foreach (var todoTasks in result)
            {
                var inProgressTask =
                    zkClient.ListChildren("/" + todoTasks.Key + "/inprogress");
                todoTasks.Value.Except(inProgressTask);
            }

            return result;
        }

        public List<string> ListUndoingTask(string taskType)
        {
            return ListUndoingTask()[taskType];
        }

        /// <summary>
        /// 获取所有已经注册的服务
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> ListAllRegisteredService()
        {
            var result = new Dictionary<string, List<string>>();
            foreach (var serviceType in ConstData.ServiceTypes)
            {
                var services = zkClient.ListChildren("/" + serviceType);
                result.Add(serviceType, services);
            }

            return result;
        }

        /// <summary>
        /// 获取特定的已经注册的服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public List<string> ListRegisteredService(string serviceType)
        {
            return ListAllRegisteredService()[serviceType];
        }

        /// <summary>
        /// 重新分发待办任务
        /// </summary>
        /// <param name="taskType"></param>
        public void DistributeTask(string taskType)
        {
            var taskList = ListUndoingTask(taskType);
            foreach (var task in taskList)
            {
                kafkaClient.SendMessageAsync(taskType, task).GetAwaiter().GetResult();
            }
        }
    }
}
