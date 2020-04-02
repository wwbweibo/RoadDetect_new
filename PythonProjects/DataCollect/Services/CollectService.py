from DataCollect.Services.GPSService import GPSService
from DataCollect.Services.CameraService import CameraService
import cv2
from threading import Thread
from PythonCoreLib.Models.TaskItemModel_pb2 import TaskItemModel
from PythonCoreLib.Models.CancellationToken import CancellationToken
from PythonCoreLib.Models.MessageTopicEnum_pb2 import MessageTopicEnum
from PythonCoreLib.Utils.Utils import encode_image_b64, encode_bytes_data_b64
from PythonCoreLib.Kafka.KafkaClient import KafkaClient
from PythonCoreLib.Redis.RedisClient import RedisClient
from uuid import uuid1
import time

class CollectService:
    def __init__(self, gpsService:GPSService, cameraService:CameraService, redisService:RedisClient, kafkaService:KafkaClient):
        self.gpsService = gpsService
        self.cameraService = cameraService
        self.gpsService.start_location()
        self.redisService = redisService
        self.kafkaService = kafkaService
        self.majorTaskId = ""

    def data_collect(self, interval:int, cancelationToken:CancellationToken):
        print("DEBUG:", cancelationToken.get_task_status())
        while not cancelationToken.get_task_status():
            location = self.gpsService.get_current_location()
            picture = self.cameraService.capture_picture()
            self.data_sender(picture, location)
            time.sleep(interval)
            

    def start_data_collect(self, cancelationToken:CancellationToken, interval:int):
        self.majorTaskId = str(uuid1())
        t = Thread(target=self.data_collect, args=(interval, cancelationToken))
        t.start()
        print("DEBUG: 开始图像采集")

    def data_sender(self, image, location):
        print("message send start",time.time())
        # 对图像进行压缩和编码
        img = encode_image_b64(image)
        subTaskId = str(uuid1())
        self.redisService.hset(self.majorTaskId,subTaskId, img)
        taskModel = TaskItemModel()
        taskModel.majorTaskId = self.majorTaskId
        taskModel.position.CopyFrom(location)
        taskModel.subTaskId = subTaskId
        taskModel.subTaskTime = time.strftime('%Y-%m-%d %H:%M:%S',time.localtime(time.time()))
        message = encode_bytes_data_b64(taskModel.SerializeToString())
        self.kafkaService.send_message(MessageTopicEnum.TaskItemData, message)
        print("message send end",time.time())

