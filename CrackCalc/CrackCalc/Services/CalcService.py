from CrackCalc.CrackCalc.Services.DeepLearningModel.MainModel import URD
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Utils.Utils import decode_bytes_image, decode_b64_image, encode_bytes_data_b64

class CalcService:
    """description of class"""
    def __init__(self,conf):
        self.Model = URD(conf['auto_encoder_weight'], conf['urd_weight'])
        self.redisClient = RedisClient(conf['redis_host'], conf['redis_port'])

    def __decode_image__(self):
        pass

    def __load_data__(self, task, dataType):
        if dataType == "taskId":
            if task is None:
                raise Exception("CalcService: the input task id is None")
            data = self.redisClient.get(task)
            if data is None:
                raise Exception("CalcService: the data get from redis is None, TaskId"+task)

    def execute_calc(self, task, dataType):
        pass

