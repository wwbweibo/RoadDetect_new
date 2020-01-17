from CrackPreProcess import app, service, conf, zkClient, serviceId, kafkaClient, logManager, serviceName, serviceProcessTask, serviceTaskListenTopic
from PythonCoreLib.Models.ControlMessageModel import ControlMessageModel
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType
import json


def onMessage(message):
    taskId = message.value.decode('utf-8')
    # 通过异步消息触发，需要请求任务
    if zkClient.require_task("preprocess", taskId, serviceId):
        try:
            service.execute_workflow(taskId)
        except Exception as e:
            zkClient.task_execute_error(serviceProcessTask, taskId)
            kafkaClient.send_message(serviceProcessTask, serviceId)
            logManager.error("Preprocess service execute work flow error", serviceId, ServiceType.PreProcessService)


def OnControllMessage(message):
    model = json.loads(message.value.decode('utf-8'), object_hook=ControlMessageModel.json2obj)
    if model.ReciveServiceId == serviceId:
        if model.ControlType == 'STOP':
            kafkaClient.StopListen()
            zkClient.stop_service()
            logManager.info("Preprocess service stopped since receive stop signal", serviceId, ServiceType.PreProcessService)


if __name__ == '__main__':
    logManager.info("Preprocess Service Online", serviceId, ServiceType.PreProcessService)
    kafkaClient.start_listen_message(serviceTaskListenTopic, onMessage, "python-preprocess-"+serviceId)
    # 接受控制消息
    kafkaClient.start_listen_message(['ControllMessage'], OnControllMessage, "python-preprocess-"+serviceId)
    # 注册该服务
    zkClient.register_service(serviceId, ServiceType.PreProcessService)
    app.run("0.0.0.0", int(conf['service_inner_port']))
