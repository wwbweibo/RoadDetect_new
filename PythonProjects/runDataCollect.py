# -*- coding:utf-8 -*-
from DataCollect import app, zkClient, kafkaClient, redisClient, logManager, serviceId, conf, collectService
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Models.CancellationToken import CancellationToken
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes, encode_image_b64
from PythonCoreLib.Models import TaskItemModel_pb2, TaskResultModel_pb2, TaskControlModel_pb2
import PythonCoreLib.Models.MessageTopicEnum_pb2 as MessageTopicEnum

taskCancellationToken = CancellationToken()

def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == serviceId:
        # 控制消息为开始图像采集
        if model.data == "START_DATA_COLLECT":
            taskCancellationToken.start_task()
            collectService.start_data_collect(taskCancellationToken,1 )
            logManager.info("开始图像采集", serviceId, ServiceType.DataCollect)
        # 控制消息为结束图像采集
        if model.data == "STOP_DATA_COLLECT":
            taskCancellationToken.cancel_task()
            logManager.info("图像采集结束", serviceId, ServiceType.DataCollect)
        if model.data == 'STOP':
            taskCancellationToken.cancel_task()
            zkClient.stop_service()
            logManager.info("Python 图像计算服务下线", serviceId, ServiceType.DataCalc)
        if model.data == "START":
            taskCancellationToken.start_task()
            kafkaClient.start_listen_message([MessageTopicEnum.TaskItemData], onMessage, "python-crackcalc-"+serviceId, taskCancellationToken)
            zkClient.register_service(serviceId, ServiceType.DataCalc)


if __name__ == "__main__":
    logManager.info("Python 图像采集服务上线", serviceId, ServiceType.DataCalc)
    # 接受控制消息
    kafkaClient.start_listen_message([MessageTopicEnum.ControlMessage], OnControllMessage, "DataCollect-"+serviceId, None)
    # 注册该服务
    zkClient.register_service(ServiceType.DataCollect, serviceId)
    app.run("0.0.0.0", int(conf['service_inner_port']))