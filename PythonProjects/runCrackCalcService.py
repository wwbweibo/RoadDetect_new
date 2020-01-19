"""
This script runs the CrackCalc application using a development server.
"""
from CrackCalc import app, service, zkClient, serviceId, kafkaClient, serviceName, serviceTaskListenTopic, serviceProcessTask, conf, logManager
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes
import json


def onMessage(message):
    taskId = message
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task(serviceProcessTask, taskId, serviceId):
        try:
            service.execute_calc(taskId)
            zkClient.finish_task(serviceProcessTask, taskId)
        except Exception as e:
            zkClient.task_execute_error(serviceProcessTask, taskId)
            kafkaClient.send_message(serviceProcessTask, taskId)
            logManager.info("CrackCalc Service execute work flow error" , serviceId, ServiceType.CrackCalcService)


def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == serviceId:
        if model.data == 'STOP':
            kafkaClient.stop_listen()
            zkClient.stop_service()
            logManager.info("Preprocess service stopped since receive stop signal", serviceId, ServiceType.PreProcessService)


if __name__ == '__main__':
    logManager.info("CrackCalc Service Online", serviceId, ServiceType.CrackCalcService)
    kafkaClient.start_listen_message(serviceTaskListenTopic, onMessage, serviceName)
    # 接受控制消息
    kafkaClient.start_listen_message(['ControllMessage'], OnControllMessage, serviceName)
    # 注册该服务
    zkClient.register_service(serviceId, 'CrackCalcService')
    app.run("0.0.0.0", int(conf['service_inner_port']))