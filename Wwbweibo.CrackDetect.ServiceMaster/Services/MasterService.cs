using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.Kafka;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Libs.Zookeeper;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class MasterService
    {
        public IKafkaService kafkaClient { get; set; }
        public ZookeeperClient zkClient { get; set; }

        /// <summary>
        /// 获取所有待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public Dictionary<TaskType, List<string>> ListAllTodoTask()
        {
            var result = new Dictionary<TaskType, List<string>>();

            foreach (TaskType task in Enum.GetValues(typeof(TaskType)))
            {
                var path =
                    ConstData.TodoTaskPath.Format((int)task + "", "");
                path = path.Remove(path.LastIndexOf('/'));
                result.Add(task, zkClient.ListChildren(path).Select(p => p.Item1).ToList());
            }

            return result;
        }

        public List<string> ListAllTodoTask(TaskType taskType)
        {
            return ListAllTodoTask()[taskType];
        }

        /// <summary>
        /// 获取没有在处理的待办任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public Dictionary<TaskType, List<string>> ListUndoingTask()
        {
            var result = ListAllTodoTask();
            foreach (var todoTasks in result)
            {
                var inProgressTaskPath = ConstData.InProgressPath.Format(todoTasks.Key.ToString(), "");
                inProgressTaskPath = inProgressTaskPath.Remove(inProgressTaskPath.LastIndexOf('/'));

                var inProgressTask =
                    zkClient.ListChildren(inProgressTaskPath).Select(p => p.Item1);
                todoTasks.Value.Except(inProgressTask);
            }

            return result;
        }

        public List<string> ListUndoingTask(TaskType taskType)
        {
            return ListUndoingTask()[taskType];
        }

        /// <summary>
        /// 获取所有已经注册的服务
        /// </summary>
        /// <returns></returns>
        public Dictionary<ServiceType, List<Tuple<string, Wwbweibo.CrackDetect.Models.ServiceStatusEnum>>> ListAllRegisteredService()
        {
            var result = new Dictionary<ServiceType, List<Tuple<string, ServiceStatusEnum>>>();
            foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
            {
                var servicePath = ConstData.ServicePath.Format((int)serviceType + "", "");
                servicePath = servicePath.Remove(servicePath.LastIndexOf('/'));
                var services = zkClient.ListChildren(servicePath);
                if(services == null)
                    continue;
                result.Add(serviceType, 
                    services.Select(p =>new Tuple<string, ServiceStatusEnum>(p.Item1, (ServiceStatusEnum)p.Item2.ToInt())).ToList());
            }

            return result;
        }

        /// <summary>
        /// 获取特定的已经注册的服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public List<Tuple<string, ServiceStatusEnum>> ListRegisteredService(ServiceType serviceType)
        {
            return ListAllRegisteredService()[serviceType];
        }

        public bool StopService(ServiceType serviceType, string serviceId)
        {
            ControlMessageModel message = new ControlMessageModel() { Data = "STOP", ReceiveServiceId = serviceId, ServiceType = serviceType };
            return kafkaClient.SendMessageAsync((int)MessageTopicEnum.ControlMessage + "", message.ToByteArray().EncodeBytesToBase64String()).Result;
        }

        /// <summary>
        /// 重新分发待办任务
        /// </summary>
        /// <param name="taskType"></param>
        public void DistributeTask(TaskType taskType)
        {
            var taskList = ListUndoingTask(taskType);
            foreach (var task in taskList)
            {
                kafkaClient.SendMessageAsync((int)MessageTopicEnum.TaskItemData + "", task).GetAwaiter().GetResult();
            }
        }
    }
}
