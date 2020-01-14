import datetime


class LogMessageModel:
    def __init__(self):
        self.LogLevel = None
        self.LogMessage = None
        self.LogTime = None
        self.Exception = None
        self.OriginServiceId=None
        self.OriginServiceType=None

    def parse2json(self, self_1):
        return {
            'LogLevel':self.LogLevel,
            'LogMessage':self.LogMessage,
            'LogTime':self.LogTime,
            'Exception':self.Exception,
            'OriginServiceId':self.OriginServiceId,
            'OriginServiceType':self.OriginServiceType
        }

    @staticmethod
    def json2obj(data):
        model = LogMessageModel()
        model.LogLevel = data['LogLevel']
        model.LogMessage = data['LogMessage']
        model.LogTime = data['LogTime']
        model.Exception = data['Exception']
        model.OriginServiceType = data['OriginServiceType']
        model.OriginServiceId = data['OriginServiceId']
        return model
