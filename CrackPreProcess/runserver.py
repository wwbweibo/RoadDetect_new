"""
This script runs the CrackPreProcess application using a development server.
"""

from os import environ
from CrackPreProcess import app
from CrackPreProcess.Kafka.Client import Client
from CrackPreProcess.Service.PreProcessService import PreProcessService

def onMessage(message):
    service = PreProcessService(message.value)
    service.ExecuteWorkFlow()

if __name__ == '__main__':
    # when server started, start kafka consumer and listen the message
    kafkaClient = Client("ali.wwbweibo.me", "9092")
    kafkaClient.StartListenMessage("preprocess", onMessage, "python-preprocess")
    app.run("0.0.0.0", 5555)
