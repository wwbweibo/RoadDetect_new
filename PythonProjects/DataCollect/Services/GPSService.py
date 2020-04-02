from DataCollect.Services.GPS.L76X import L76X
import math
import time
from threading import Thread
from PythonCoreLib.Models.TaskItemModel_pb2 import Position

class GPSService:
    def __init__(self):
        self.x = None
        self.x=L76X()
        self.x.L76X_Set_Baudrate(9600)
        self.x.L76X_Send_Command(self.x.SET_NMEA_BAUDRATE_115200)
        time.sleep(2)
        self.x.L76X_Set_Baudrate(115200)

        self.x.L76X_Send_Command(self.x.SET_POS_FIX_400MS)

        #Set output message
        self.x.L76X_Send_Command(self.x.SET_NMEA_OUTPUT)

        self.x.L76X_Exit_BackupMode()

        self.Lon = 0.0
        self.Lat = 0.0


    def get_location(self):
        while True:
            self.x.L76X_Gat_GNRMC()
            if self.x.Status == 1:
                self.x.L76X_Baidu_Coordinates(self.x.Lat, self.x.Lon)
                print(time.strftime('%Y-%m-%d %H:%M:%S',time.localtime(time.time())), self.x.Lon_Baidu, self.x.Lat_Baidu, '\n'  )
                self.Lon, self.Lat  = self.x.Lon_Baidu, self.x.Lat_Baidu
            else:
                self.Lon, self.Lat = 0, 0

    def start_location(self):
        taskThread = Thread(target=self.get_location)
        taskThread.start()

    def get_current_location(self) -> Position:
        pos = Position()
        pos.longitude = self.Lon
        pos.latitude = self.Lat
        return pos
    