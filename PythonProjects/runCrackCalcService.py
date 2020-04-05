from CrackCalc import app, preProcessService, calcService, conf, zkClient, kafkaClient, logManager,redisClient
from CrackCalc import service_name, service_id
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Models.CancellationToken import CancellationToken
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes, encode_image_b64, encode_bytes_data_b64
from PythonCoreLib.Models import TaskItemModel_pb2, TaskResultModel_pb2
import PythonCoreLib.Models.MessageTopicEnum_pb2 as MessageTopicEnum

taskCancellationToken = CancellationToken()


def onMessage(message):
    model = TaskItemModel_pb2.TaskItemModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.subTaskId is not None:
        # 通过异步消息触发，需要请求任务
        if zkClient.require_task( model.majorTaskId , model.subTaskId, service_id):
            print("receive message start to calc:", model.majorTaskId, model.subTaskId)
            try:
                image_block, image = preProcessService.execute_workflow(model)
                isCrack,im = calcService.execute_calc(image_block, image)
                resultModel = TaskResultModel_pb2.TaskResultModel()
                resultModel.majorTaskId = model.majorTaskId
                resultModel.subTaskId = model.subTaskId
                resultModel.crackArea = 3.0
                if isCrack:
                    import cv2
                    redisClient.hset(model.majorTaskId, "result-"+model.subTaskId, encode_image_b64(im))
                    resultModel.isCrack = True
                else:
                    resultModel.isCrack = False
                zkClient.finish_task(model.majorTaskId, model.subTaskId)
                kafkaClient.send_message(MessageTopicEnum.TaskCalc, encode_bytes_data_b64(resultModel.SerializeToString()))
                print("calc finished:", model.majorTaskId, model.subTaskId)
            except Exception as e:
                zkClient.task_execute_error(model.majorTaskId, model.subTaskId)
                print("calc faild", model.majorTaskId, model.subTaskId, e)
                logManager.error("service execute work flow error", service_id, ServiceType.DataCalc)
    else:
        logManager.error("获取到的信息异常")


def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == service_id:
        if model.data == 'STOP':
            taskCancellationToken.cancel_task()
            zkClient.stop_service()
            logManager.info("Python 图像计算服务下线", service_id, ServiceType.DataCalc)
        if model.data == "START":
            taskCancellationToken.start_task()
            kafkaClient.start_listen_message([MessageTopicEnum.TaskItemData], onMessage, "python-crackcalc-"+service_id, taskCancellationToken)
            zkClient.start_service(service_id, ServiceType.DataCalc)


if __name__ == "__main__":
    kafkaClient.start_listen_message([MessageTopicEnum.TaskItemData], onMessage, "DataCalc-"+service_id, taskCancellationToken)
    # 接受控制消息
    kafkaClient.start_listen_message([MessageTopicEnum.ControlMessage], OnControllMessage, "DataCalc-"+service_id, None)
    # 注册该服务
    zkClient.register_service(ServiceType.DataCalc, service_id)
    logManager.info("Python 图像计算服务上线", service_id, ServiceType.DataCalc)
    app.run("0.0.0.0", int(conf['service_inner_port']))
