import base64
import cv2
import os
from os import path
import numpy as np


def decode_b64_image(data):
    return base64.b64decode(data)


def encode_bytes_data_b64(data):
    return base64.b64encode(data)


def decode_bytes_image(data):
    """
    convert a  byte like data to opencv image
    """
    data = np.frombuffer(data, dtype=np.uint8)
    return cv2.imdecode(data, cv2.IMREAD_ANYCOLOR)


if __name__ == "__main__":
    image = open("test.jpg", 'rb')
    image = image.read()
    b64 = encode_bytes_data_b64(image)
    bytes = decode_b64_image(b64)
    image = decode_bytes_image(bytes)
    cv2.imshow("test", image)
    cv2.waitKey()