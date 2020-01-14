import base64
import numpy as np
from cv2 import cv2


def decode_b64_to_bytes(data):
    """
    Decode Base64 string to Bytes
    :param data: input base64 data
    :return: bytes
    """
    return base64.b64decode(data)


def encode_bytes_data_b64(data):
    """
    Encode Bytes to base64 string
    :param data: input bytes data
    :return: str
    """
    return base64.b64encode(data)


def decode_bytes_numpy_array(data):
    """
    decode bytes to numpy array
    :param data: input bytes data
    :return: numpy array
    """
    return np.frombuffer(data, dtype=np.uint8)


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
    bytes = decode_b64_to_bytes(b64)
    image = decode_bytes_image(bytes)
    cv2.imshow("test", image)
    cv2.waitKey()