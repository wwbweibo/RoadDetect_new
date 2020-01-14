class ControlMessageModel(object):
    def __init__(self):
        self.ReciveServiveId=None
        self.ControlType=None

    def parse2json(self, self_1):
        return {
            'ReciveServiceId':self.ReciveServiveId,
            'ControlType':self.ControlType
                }
    @staticmethod
    def json2obj(data):
        model = ControlMessageModel()
        model.ControlType = data["ControlType"]
        model.ReciveServiveId = data["ControlType"]
        return model

if __name__ == "__main__":
    import json 
    model = ControlMessageModel()
    model.ControlType = "STOP"
    model.ReciveServiveId = "1233"
    jsondata = json.dumps(model, default=model.parse2json)

    r = json.loads(jsondata, object_hook=ControlMessageModel.json2obj)
    print(r.ReciveServiveId)