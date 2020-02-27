from CrackCalc import app, preProcessService, calcService, conf, zkClient, serviceId, kafkaClient, logManager
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Models.CancellationToken import CancellationToken
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes
from PythonCoreLib.Models import TaskModel_pb2
import PythonCoreLib.Models.MessageTopicEnum_pb2 as MessageTopicEnum

taskCancellationToken = CancellationToken()


def onMessage(message):
    model = TaskModel_pb2.TaskModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.subTaskId is not None:
        # 通过异步消息触发，需要请求任务
        if zkClient.require_task( model.majorTaskId , model.subTaskId, serviceId):
            try:
                image_block, _ = preProcessService.execute_workflow(model)
                calcService.execute_calc(image_block)
            except Exception:
                zkClient.task_execute_error(model.majorTaskId, model.subTaskId)
                logManager.error("service execute work flow error", serviceId, ServiceType.DataCalc)
    else:
        logManager.error("获取到的信息异常")


def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == serviceId:
        if model.data == 'STOP':
            taskCancellationToken.cancel_task()
            zkClient.stop_service()
            logManager.info("Python 图像计算服务下线", serviceId, ServiceType.DataCalc)
        if model.data == "START":
            taskCancellationToken.start_task()
            kafkaClient.start_listen_message([MessageTopicEnum.TaskItemData], onMessage, "python-crackcalc-"+serviceId, taskCancellationToken)
            zkClient.register_service(serviceId, ServiceType.DataCalc)


if __name__ == "__main__":
    logManager.info("Python 图像计算服务上线", serviceId, ServiceType.DataCalc)
    kafkaClient.start_listen_message([MessageTopicEnum.TaskItemData], onMessage, "DataCalc-"+serviceId, taskCancellationToken)
    # 接受控制消息
    kafkaClient.start_listen_message([MessageTopicEnum.ControlMessage], OnControllMessage, "DataCalc-"+serviceId, None)
    # 注册该服务
    zkClient.register_service(ServiceType.DataCalc, serviceId)
    app.run("0.0.0.0", int(conf['service_inner_port']))
