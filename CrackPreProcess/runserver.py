from os import environ
from CrackPreProcess import app, service, conf, zkClient, serviceId, kafkaClient
from CrackPreProcess.Kafka.Client import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService
from CrackPreProcess.Models.ControlMessageModel import ControlMessageModel
import json

def onMessage(message):
    taskId = message.value.decode('utf-8')
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task("preprocess", taskId, serviceId):
        try:
            service.execute_workflow(taskId)
        except Exception as e:
            zkClient.finish_task("preprocess", taskId)

def OnControllMessage(message):
    model = json.loads(message.value.decode('utf-8'), object_hook=ControlMessageModel.json2obj)
    if model.ReciveServiceId == serviceId:
        if model.ControlType == 'STOP':
            kafkaClient.StopListen()
            zkClient.stop_service()

if __name__ == '__main__':
    kafkaClient.StartListenMessage(["preprocess"], onMessage, "python-preprocess-"+serviceId)
    # 接受控制消息
    kafkaClient.StartListenMessage(['ControllMessage'], OnControllMessage, "python-preprocess-"+sserviceId)
    # 注册该服务
    zkClient.register_service(serviceId, "python-preprocess-service")
    app.run("0.0.0.0", conf['service_inner_port'])
