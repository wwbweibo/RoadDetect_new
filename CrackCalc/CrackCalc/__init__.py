"""
The flask application package.
"""

from flask import Flask
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from CrackCalc.CrackCalc.Services.CalcService import CalcService
import uuid
import CrackCalc.CrackCalc.views


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
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
redisClient = RedisClient(conf['redis_host'], conf['redis_client'])
app = Flask(__name__)

serviceProcessTask = "crackcalc"
serviceName = "python-crackcalc-"+serviceId
serviceTaskListenTopic = ['crackcalc']



