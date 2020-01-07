from os import environ
from CrackPreProcess import app, service, conf, zkClient, serviceId
from CrackPreProcess.Kafka.Client import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService
import json

def onMessage(message):
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task("preprocess", message.value, serviceId.encode('utf-8')):
        task = json.loads(message.value)
        service.execute_workflow(task)

if __name__ == '__main__':
    # when server started, start kafka consumer and listen the message
    kafkaClient = Client(conf['kafka_host'], conf['kafka_port'])
    kafkaClient.StartListenMessage(["preprocess"], onMessage, "python-preprocess")
    # 注册该服务
    zkClient.register_service(serviceId, "python-preprocess-service")
    app.run("0.0.0.0", 5555)
