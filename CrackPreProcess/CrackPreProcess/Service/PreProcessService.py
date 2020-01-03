import cv2
from CrackPreProcess.Utils.Utils import Decodeb64String, DecodeByte2Image

'''
流程待定
'''
class PreProcessService(object):
    """description of class"""
    def __init__(self, base64string):
        self.imageb64String = base64string
        self.image = None
        self.__decode_image__()
        
    def __decode_image__(self):
        bytedata = Decodeb64String(self.imageb64String)
        self.image = DecodeByte2Image(bytedata)
        if(self.image.shape[0] != self.image.shape[1] != 1024):
            raise("input image shape error, require 1024 * 1024 image")

    def ExecuteWorkFlow(self):
        return self.image.shape

    def CutImage():
        im_list = []
        for i in range(64):
            for j in  range(64):
                im_list.append(self.image[i * 16: (i+1) * 16, j * 16: (j+1) * 16, :])
        self.image_block = im_list