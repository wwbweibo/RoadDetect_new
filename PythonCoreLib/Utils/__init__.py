import base64

def Decodeb64String(data):
    return base64.b64decode(data)

def EncodeString2b64(data):
    return base64.b64encode(data)