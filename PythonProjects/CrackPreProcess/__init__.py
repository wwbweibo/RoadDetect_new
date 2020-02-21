"""
The flask application package.
"""
from flask import Flask
from CrackPreProcess.Service.PreProcessService import PreProcessService
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Utils.Logging import LogManager
from PythonCoreLib.Utils.Utils import load_conf

conf = load_conf('CrackPreProcess/conf.ini')
app = Flask(__name__)
service = PreProcessService(conf)
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
serviceId = conf['service_id']
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
logManager = LogManager(conf)

serviceProcessTask = "PreProcess"
serviceName = "python-preprocess-"+serviceId
serviceTaskListenTopic = ['PreProcess']

import CrackPreProcess.views
