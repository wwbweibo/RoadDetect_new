from CrackCalc.Services.DeepLearningModel.MainModel import URD
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Utils.Utils import decode_bytes_image, decode_b64_to_bytes, encode_bytes_data_b64, decode_bytes_numpy_array


class CalcService:
    """description of class"""
    def __init__(self,conf):
        self.Model = URD(conf['auto_encoder_weight'], conf['urd_weight'])
        self.redisClient = RedisClient(conf['redis_host'], conf['redis_port'])

    def __decode_image__(self, data):
        byteData = decode_b64_to_bytes(data)
        return decode_bytes_numpy_array(byteData)

    def __load_data__(self, task, dataType):
        if dataType == "taskId":
            if task is None:
                raise Exception("CalcService: the input task id is None")
            data = self.redisClient.get(task)
            if data is None:
                raise Exception("CalcService: the data get from redis is None, TaskId"+task)
            return self.__decode_image__(data)
        else:
            if task is None:
                raise Exception("CalcService: the input task is None")
            return self.__decode_image__(task)

    def execute_calc(self, task, dataType):
        data = self.__load_data__(task, dataType)
        self.Model.execute_calc(data)