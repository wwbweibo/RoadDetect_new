from os import environ
from CrackPreProcess import app, service, conf, zkClient, serviceId
from CrackPreProcess.Kafka.Client import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService
import json

def onMessage(message):
    taskId = message.value.decode('utf-8')
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task("preprocess", taskId, serviceId):
        try:
            service.execute_workflow(taskId)
        except Exception as e:
            zkClient.finish_task("preprocess", taskId)

if __name__ == '__main__':
    # when server started, start kafka consumer and listen the message
    kafkaClient = Client(conf['kafka_host'], conf['kafka_port'])
    # 使用唯一的GroupID确保所有的服务都能够能接受到消息
    kafkaClient.StartListenMessage(["preprocess"], onMessage, "python-preprocess-"+serviceId)
    # 注册该服务
    zkClient.register_service(serviceId, "python-preprocess-service")
    app.run("0.0.0.0", 5555)
