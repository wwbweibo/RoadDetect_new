"""
This script runs the CrackCalc application using a development server.
"""

from os import environ
from CrackCalc.CrackCalc import app, service, zkClient, serviceId, kafkaClient, serviceName, serviceTaskListenTopic, serviceProcessTask, conf
from PythonCoreLib.Models.ControlMessageModel import  ControlMessageModel
import json


def onMessage(message):
    taskId = message.value.decode('utf-8')
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task(serviceProcessTask, taskId, serviceId):
        try:
            service.execute_workflow(taskId)
            zkClient.finish_task(serviceProcessTask, taskId)
        except Exception as e:
            zkClient.task_execute_error(serviceProcessTask, taskId)
            kafkaClient.send_message(serviceProcessTask, taskId)


def OnControllMessage(message):
    model = json.loads(message.value.decode('utf-8'), object_hook=ControlMessageModel.json2obj)
    if model.ReciveServiceId == serviceId:
        if model.ControlType == 'STOP':
            kafkaClient.stop_listen()
            zkClient.stop_service()


if __name__ == '__main__':
    kafkaClient.start_listen_message(serviceTaskListenTopic, onMessage, serviceName)
    # 接受控制消息
    kafkaClient.start_listen_message(['ControllMessage'], OnControllMessage, serviceName)
    # 注册该服务
    zkClient.register_service(serviceId, "python-crackcalc-service")
    app.run("0.0.0.0", conf['service_inner_port'])