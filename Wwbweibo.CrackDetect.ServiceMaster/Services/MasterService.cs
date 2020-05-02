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

        #region 服务控制方案
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public bool StopService(ServiceType serviceType, string serviceId)
        {
            ControlMessageModel message = new ControlMessageModel() { Data = "STOP", ReceiveServiceId = serviceId, ServiceType = serviceType };
            return kafkaClient.SendMessageAsync((int)MessageTopicEnum.ControlMessage + "", message.ToByteArray().EncodeBytesToBase64String()).Result;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        public void StartService(ServiceType serviceType, string serviceId)
        {
            ControlMessageModel message = new ControlMessageModel() { Data = "START", ReceiveServiceId = serviceId, ServiceType = serviceType };
            kafkaClient.SendMessageAsync((int)MessageTopicEnum.ControlMessage + "",
                message.ToByteArray().EncodeBytesToBase64String());
        }
        /// <summary>
        /// 启动数据采集
        /// </summary>
        /// <param name="serviceId"></param>
        public void StartDataCollect(string serviceId)
        {
            ControlMessageModel message = new ControlMessageModel() { Data = "START_DATA_COLLECT", ReceiveServiceId = serviceId, ServiceType = ServiceType.DataCollect };
            kafkaClient.SendMessageAsync((int)MessageTopicEnum.ControlMessage + "",
                message.ToByteArray().EncodeBytesToBase64String());
        }

        /// <summary>
        /// 停止数据采集
        /// </summary>
        /// <param name="serviceId"></param>
        public void StopDataCollect(string serviceId)
        {
            ControlMessageModel message = new ControlMessageModel() { Data = "STOP_DATA_COLLECT", ReceiveServiceId = serviceId, ServiceType = ServiceType.DataCollect };
            kafkaClient.SendMessageAsync((int)MessageTopicEnum.ControlMessage + "",
                message.ToByteArray().EncodeBytesToBase64String());
        }
        #endregion


        #region 服务发现方案
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
                var services = zkClient.ListChildren(servicePath).Result;
                if (services == null)
                    continue;
                result.Add(serviceType,
                    services.Select(p => new Tuple<string, ServiceStatusEnum>(p.Item1, (ServiceStatusEnum)p.Item2.ToInt())).ToList());
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
        #endregion
        // todo: 添加更多的服务管理方法
    }
}
