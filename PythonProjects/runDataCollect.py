# -*- coding:utf-8 -*-
from DataCollect import app, zkClient, kafkaClient, redisClient, logManager, serviceId, conf, collectService
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Models.CancellationToken import CancellationToken
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
from PythonCoreLib.Models.ServiceStatusEnum_pb2 import ServiceStatusEnum
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes, encode_image_b64
from PythonCoreLib.Models import TaskItemModel_pb2, TaskResultModel_pb2, TaskControlModel_pb2
import PythonCoreLib.Models.MessageTopicEnum_pb2 as MessageTopicEnum

taskCancellationToken = CancellationToken()
currentStatus = ServiceStatusEnum.Idle

def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == serviceId:
        print(model)
        # 控制消息为开始图像采集
        if model.data == "START_DATA_COLLECT":
            print(currentStatus, "receive message to start data collect")
            if currentStatus == ServiceStatusEnum.Idle:
                zkClient.running_service()
                taskCancellationToken.start_task()
                collectService.start_data_collect(taskCancellationToken,1 )
                logManager.info("开始图像采集", serviceId, ServiceType.DataCollect)
        # 控制消息为结束图像采集
        if model.data == "STOP_DATA_COLLECT":
            print("revice message to stop data collect")
            taskCancellationToken.cancel_task()
            zkClient.idle_service()
            logManager.info("图像采集结束", serviceId, ServiceType.DataCollect)
        if model.data == 'STOP':
            print(model.receiveServiceId)
            taskCancellationToken.cancel_task()
            zkClient.stop_service()
            logManager.info("Python 图像采集服务下线", serviceId, ServiceType.DataCollect)
        if model.data == "START":
            taskCancellationToken.start_task()
            logManager.info("Python 图像采集服务上线", serviceId, ServiceType.DataCollect)
            zkClient.start_service()


if __name__ == "__main__":
    logManager.info("Python 图像采集服务上线", serviceId, ServiceType.DataCollect)
    # 接受控制消息
    kafkaClient.start_listen_message([MessageTopicEnum.ControlMessage], OnControllMessage, "DataCollect-"+serviceId, None)
    # 注册该服务
    zkClient.register_service(ServiceType.DataCollect, serviceId)
    app.run("0.0.0.0", int(conf['service_inner_port']))