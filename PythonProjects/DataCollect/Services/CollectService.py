from DataCollect.Services.GPSService import GPSService
from DataCollect.Services.CameraService import CameraService
import cv2

class CollectService:
    def __init__(self, gpsService, cameraService):
        self.gpsService = gpsService
        self.cameraService = cameraService
        self.gpsService.start_location()

    def data_collect(self):
        lon,lat = self.gpsService.get_current_location()
        picture = self.cameraService.capture_picture()
        print(lon, lat)
        cv2.imshow("",picture)
        cv2.waitKey()

if __name__ == "__main__":
    service = CollectService()
    service.data_collect()
