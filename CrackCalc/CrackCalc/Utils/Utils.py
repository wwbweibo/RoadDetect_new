import base64
import cv2
import os
from os import path
import numpy as np

def Decodeb64String(data):
    return base64.b64decode(data)

def EncodeData2b64(data):
    return base64.b64encode(data)

def DecodeByte2Image(data):
    '''
    convert a  byte like data to opencv image
    '''
    data = np.frombuffer(data, dtype=np.uint8)
    return cv2.imdecode(data, cv2.IMREAD_ANYCOLOR)

if __name__ == "__main__":
    image = open("test.jpg", 'rb')
    image = image.read()
    b64 = EncodeData2b64(image)
    bytes = Decodeb64String(b64)
    image = DecodeByte2Image(bytes)
    cv2.imshow("test", image)
    cv2.waitKey()