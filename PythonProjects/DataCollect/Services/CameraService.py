from picamera import PiCamera 
import io
import cv2
import numpy as  np  

class CameraService():
    def __init__(self):
        self.Camera = PiCamera()
        self.Camera.resolution = (1024,1024)
        self.framerate = 25

    
    def capture_picture(self):
        stream = io.BytesIO()
        for f in self.Camera.capture_continuous(stream, format='jpeg', use_video_port=True):
            data = np.fromstring(stream.getvalue(), dtype=np.uint8)
            frame = cv2.imdecode(data, cv2.IMREAD_COLOR)
            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
                        
            stream.seek(0)
            return gray