from os import environ
from CrackPreProcess import app, service, conf
from CrackPreProcess.Kafka.Client import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService
import json

def onMessage(message):
    task = json.loads(message.value)
    service.execute_workflow(task)

if __name__ == '__main__':
    # when server started, start kafka consumer and listen the message
    kafkaClient = Client(conf['kafka_host'], conf['kafka_port'])
    kafkaClient.StartListenMessage(["preprocess"], onMessage, "python-preprocess")
    app.run("0.0.0.0", 5555)
