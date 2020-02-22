from CrackCalc import app, preProcessService, calcService, conf, zkClient, serviceId, kafkaClient, logManager, serviceName, serviceProcessTask, serviceTaskListenTopic
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
from PythonCoreLib.Models.ControlMessageModel_pb2 import ControlMessageModel
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes
import json


def onMessage(message):
    taskId = message
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task(serviceProcessTask, taskId, serviceId):
        try:
            image_block, _ = preProcessService.execute_workflow(taskId)
            calcService.execute_calc(image_block)
        except Exception:
            zkClient.task_execute_error(serviceProcessTask, taskId)
            kafkaClient.send_message(serviceProcessTask, serviceId)
            logManager.error("service execute work flow error", serviceId, ServiceType.CrackCalcService)


def OnControllMessage(message):
    model = ControlMessageModel()
    model.ParseFromString(decode_b64_to_bytes(message))
    if model.receiveServiceId == serviceId:
        if model.data == 'STOP':
            kafkaClient.stop_listen()
            zkClient.stop_service()
            logManager.info("Preprocess service stopped since receive stop signal", serviceId, ServiceType.PreProcessService)


if __name__ == '__main__':
    logManager.info("Preprocess Service Online", serviceId, ServiceType.PreProcessService)
    kafkaClient.start_listen_message(serviceTaskListenTopic, onMessage, "python-preprocess-"+serviceId)
    # 接受控制消息
    kafkaClient.start_listen_message(['ControllMessage'], OnControllMessage, "python-preprocess-"+serviceId)
    # 注册该服务
    zkClient.register_service(serviceId, 'PreProcessService')
    app.run("0.0.0.0", int(conf['service_inner_port']))
