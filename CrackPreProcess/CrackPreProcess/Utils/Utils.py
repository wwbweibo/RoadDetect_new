import base64
import cv2

def Decodeb64String(data):
    return base64.b64decode(data)

def EncodeString2b64(data):
    return base64.b64encode(data)

def DecodeByte2Image(data):
    cv2.imdecode(data)