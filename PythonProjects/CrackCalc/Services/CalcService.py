from CrackCalc.Services.DeepLearningModel.MainModel import URD
from PythonCoreLib.Redis.RedisClient import RedisClient
from PythonCoreLib.Utils.Utils import decode_bytes_image, decode_b64_to_bytes, encode_bytes_data_b64, decode_bytes_numpy_array


class CalcService:
    """description of class"""
    def __init__(self,conf):
        self.Model = URD(conf['auto_encoder_weight'], conf['urd_weight'])
        self.redisClient = RedisClient(conf['redis_host'], conf['redis_port'])

    def execute_calc(self, data):
        result = self.Model.execute_calc(data)
        crackNumbers = result.where(result == 0)[0]
        return crackNumbers