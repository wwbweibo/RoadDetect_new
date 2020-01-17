import datetime
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Models.LogModel_pb2 import LogModel
import PythonCoreLib.Models.LogLevel_pb2 as LogLevel


class LogManager:
    def __init__(self, config):
        self.redisClient = RedisClient(config['redis_host'], config['redis_port'])

    def info(self, message, serviceId, serviceType):
        model = LogModel()
        model.LogLevel = LogLevel.Info
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = ""
        self.__send_data__(model)

    def warning(self, message, serviceId, serviceType):
        model = LogModel()
        model.LogLevel = LogLevel.Warning
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = ""
        self.__send_data__(model)

    def error(self, message, serviceId, serviceType, exception=""):
        model = LogModel()
        model.LogLevel = LogLevel.Error
        model.LogTime = str(datetime.datetime.now())
        model.LogMessage = message
        model.OriginServiceId = serviceId
        model.OriginServiceType = serviceType
        model.Exception = exception
        self.__send_data__(model)

    def __send_data__(self, model):
        self.redisClient.lpush(str(model.OriginServiceType) + str(model.LogLevel), model.SerializeToString())
