from CrackCalc.Services.DeepLearningModel.MainModel import URD
from PythonCoreLib.Redis.RedisClient import RedisClient
import numpy as np
import cv2

class CalcService:
    """description of class"""
    def __init__(self,conf):
        self.Model = URD(conf['auto_encoder_weight'], conf['urd_weight'])
        self.redisClient = RedisClient(conf['redis_host'], conf['redis_port'])

    def execute_calc(self, data, image):
        result = self.Model.execute_calc(data)
        idx = np.where(result == 0)[0]
        if len(idx) > 0:
            im = self.draw_bounding_box(image, idx)
            return True, im
        return False, None

    def draw_bounding_box(self, image, idx):
        for i in idx:
            y = int(i / 64) * 16
            x = (i % 64) * 16
            image = cv2.rectangle(image, (x,y), (x+16,y+16),0,1)
        image = cv2.resize(image, (512,512))
        return image