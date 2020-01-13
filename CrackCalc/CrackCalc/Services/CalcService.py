from CrackCalc.Service.DeepLearningModel.MainModel import URD
from CrackCalc import redisClient

class CalcService:
    """description of class"""
    def __init__(self,conf):
        self.Model = URD(conf['auto_encoder_weight'], conf['urd_weight'])

    def __load_data__(self, task, dataType):
        if taskType == "taskId":
            if task is not None:
                data = redisClient.Get(task)
      
    def execute_calc(self, task, dataType):
        pass

