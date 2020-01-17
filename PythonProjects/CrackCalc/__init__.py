"""
The flask application package.
"""

from flask import Flask
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Utils.Logging import LogManager
from CrackCalc.Services.CalcService import CalcService
import uuid
import tensorflow as tf



def load_conf():
    '''
    load conf from load file system
    '''
    conf = open('CrackCalc/conf.ini', 'r').readlines()
    conf_dict = dict()
    for line in conf:
        conf_name, conf_value = line.split('=')
        conf_dict[conf_name.replace(' ', '')] = conf_value.replace(' ', '').replace('\n','') 
    return conf_dict


conf = load_conf()


service = CalcService(conf)
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
serviceId = str(uuid.uuid1())
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
redisClient = RedisClient(conf['redis_host'], conf['redis_port'])
logManager = LogManager(conf)
app = Flask(__name__)
graph = tf.get_default_graph()

serviceProcessTask = "CrackCalc"
serviceName = "python-crackcalc-"+serviceId
serviceTaskListenTopic = ['CrackCalc']

import CrackCalc.views

