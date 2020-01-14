"""
The flask application package.
"""
from flask import Flask
from CrackPreProcess.Service.PreProcessService import PreProcessService
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Utils.Logging import LogManager
import uuid


def load_conf():
    '''
    load conf from load file system
    '''
    conf = open('CrackPreProcess/conf.ini', 'r').readlines()
    conf_dict = dict()
    for line in conf:
        conf_name, conf_value = line.split('=')
        conf_dict[conf_name.replace(' ', '')] = conf_value.replace(' ', '').replace('\n','') 
    return conf_dict


conf = load_conf()
app = Flask(__name__)
service = PreProcessService(conf)
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
serviceId = str(uuid.uuid1())
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
logManager = LogManager(conf)

serviceProcessTask = "preprocess"
serviceName = "python-preprocess-"+serviceId
serviceTaskListenTopic = ['preprocess']

import CrackPreProcess.views


