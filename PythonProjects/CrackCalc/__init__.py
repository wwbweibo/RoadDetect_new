"""
The flask application package.
"""

from flask import Flask
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Utils.Logging import LogManager
from CrackCalc.Services.CalcService import CalcService
from PythonCoreLib.Utils.Utils import load_conf
import uuid
import tensorflow as tf

conf = load_conf('CrackCalc/conf.ini')

service = CalcService(conf)
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
serviceId = conf['service_id']
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
redisClient = RedisClient(conf['redis_host'], conf['redis_port'])
logManager = LogManager(conf)
app = Flask(__name__)
graph = tf.get_default_graph()

serviceProcessTask = "CrackCalc"
serviceName = "python-crackcalc-"+serviceId
serviceTaskListenTopic = ['CrackCalc']

import CrackCalc.views

