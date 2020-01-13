"""
The flask application package.
"""

from flask import Flask
from CrackCalc.Kafka.Client import Client
from CrackCalc.Redis.RedisClient import RedisClient
from CrackCalc.Zookeeper.ZkClient import ZkClient
from CrackCalc.Services.CalcService import CalcService
import uuid

def load_conf():
    '''
    load conf from load file system
    '''
    conf = open('conf.ini', 'r').readlines()
    conf_dict = dict()
    for line in conf:
        conf_name, conf_value = line.split('=')
        conf_dict[conf_name.replace(' ', '')] = conf_value.replace(' ', '').replace('\n','') 
    return conf_dict

conf = load_conf()

service = CalcService(conf)
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
serviceId = str(uuid.uuid1())
kafkaClient = Client(conf['kafka_host'], conf['kafka_port'])
app = Flask(__name__)

serviceProcessTask = "crackcalc"
serviceName = "python-crackcalc-"+serviceId
serviceTaskListenTopic = ['crackcalc']

import CrackCalc.views
