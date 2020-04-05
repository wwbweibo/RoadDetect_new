# -*- coding:utf-8 -*-
"""
The flask application package.
"""

from flask import Flask
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Zookeeper.ZkClient import ZkClient
from PythonCoreLib.Utils.Logging import LogManager
from PythonCoreLib.Utils.Utils import load_conf
from DataCollect.Services.GPSService import GPSService
from DataCollect.Services.CameraService import CameraService
from DataCollect.Services.CollectService import CollectService

conf = load_conf('DataCollect/conf.ini')

# 启动必要的服务
zkClient = ZkClient([conf['zookeeper_host']],[conf['zookeeper_port']])
kafkaClient = KafkaClient(conf['kafka_host'], conf['kafka_port'])
redisClient = RedisClient(conf['redis_host'], conf['redis_port'])
logManager = LogManager(conf)


# todo: 添加添加GPS和图像采集的服务
gpsService = GPSService()
cameraService = CameraService()
collectService = CollectService(gpsService, cameraService, redisClient, kafkaClient, zkClient)

serviceId = conf['service_id']
app = Flask(__name__)

import DataCollect.views

