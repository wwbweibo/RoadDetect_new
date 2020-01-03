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
    kafkaClent = Client("ali.wwbweibo.me", "9092")
    Client.StartListenMessage("preprocess", onMessage, "python-preprocess")
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
    app.run(HOST, PORT)
