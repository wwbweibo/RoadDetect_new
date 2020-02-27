"""
The flask application package.
"""

from flask import Flask
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Utils.Logging import LogManager
from CrackCalc.Services.CalcService import CalcService
from CrackCalc.Services.CalcPreprocessService import PreProcessService
from PythonCoreLib.Utils.Utils import load_conf
import tensorflow as tf

conf = load_conf('CrackCalc/conf.ini')

# 计算服务
calcService = CalcService(conf)
# 预处理服务
preProcessService = PreProcessService(conf)
# 启动必要的服务
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
redisClient = RedisClient(conf['redis_host'], conf['redis_port'])
logManager = LogManager(conf)

serviceId = conf['service_id']
app = Flask(__name__)
graph = tf.get_default_graph()

import CrackCalc.views

