import logging
import datetime
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Models.LogMessageModel import LogMessageModel
import json


class LogManager:
    def __init__(self, config):
        self.redisClient = RedisClient(config['redis_host'], config['redis_port'])

    def info(self, message, serviceId, serviceType):
        model = LogMessageModel()
        model.LogLevel = "INFO"
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = ""
        self.__send_data__(model)

    def warning(self, message, serviceId, serviceType):
        model = LogMessageModel()
        model.LogLevel = "WARNING"
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = ""
        self.__send_data__(model)

    def error(self, message, serviceId, serviceType, exception=""):
        model = LogMessageModel()
        model.LogLevel = "INFO"
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = exception
        self.__send_data__(model)

    def __send_data__(self, model):
        self.redisClient.lpush(model.OriginServiceType + model.LogLevel, json.dumps(model, default=model.parse2json))
