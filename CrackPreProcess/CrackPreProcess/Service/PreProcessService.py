import cv2
from CrackPreProcess.Utils.Utils import Decodeb64String, DecodeByte2Image

'''
流程待定
'''
class PreProcessService(object):
    def __init__(self, base64string):
        self.imageb64String = base64string
        self.__decode_image__()
        
    def __decode_image__(self):
        bytedata = Decodeb64String(self.imageb64String)
        self.image = DecodeByte2Image(bytedata)

    """description of class"""
    def ExecuteWorkFlow(image):
        pass
    
  