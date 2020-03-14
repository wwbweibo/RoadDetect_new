import base64
import numpy as np
from cv2 import cv2
import uuid
import os.path


def decode_b64_to_bytes(data):
    """
    将base64编码解码到byte数组
    """
    return base64.b64decode(data)


def encode_bytes_data_b64(data):
    """
    将byte数组进行base64编码
    """
    return base64.b64encode(data)


def encode_image_b64(np_arr):
    """
    将OpenCv图像压缩并进行base64编码
    """
    return encode_bytes_data_b64(cv2.imencode(".jpg", np_arr))


def decode_bytes_numpy_array(data):
    """
    将byte数组转化为numpy数组
    """
    return np.frombuffer(data, dtype=np.uint8)


def decode_bytes_image(data):
    """
    将byte数组转化为OpenCv图像
    """
    data = np.frombuffer(data, dtype=np.uint8)
    return cv2.imdecode(data, cv2.IMREAD_ANYCOLOR)


def load_conf(conf_file) -> dict:
    """
    服务配置文件加载方法
    :param conf_file: 配置文件路径
    :return: 服务配置文件字典
    """
    if not os.path.exists(conf_file):
        raise Exception("无法找到服务配置文件！！！")
    conf_dict = conf_file_reader(conf_file)
    return conf_dict


def load_cus_conf(conf_path: str) -> dict:
    """
    自定义配置文件加载方法
    :param conf_path: 文件路径
    :return: 自定义配置字典
    """
    if os.path.exists(conf_path):
        conf_dict = conf_file_reader(conf_path)
        if 'service_id' not in conf_dict.keys():
            service_id = str(uuid.uuid1())
            conf_dict['service_id'] = service_id
            conf_file_writer({"service_id": service_id}, conf_path)
        if 'service_name' not in conf_dict.keys():
            service_name = "service-"+conf_dict['service_id']
            conf_dict['service_name'] = service_name
            conf_file_writer({'service_name': service_name}, conf_path)
        return conf_dict
    else:
        service_id = str(uuid.uuid1())
        service_name = "service-"+service_id
        conf_dict = {'service_id':service_id, 'service_name':service_name}
        conf_file_writer(conf_dict, conf_path)
        return conf_dict


def conf_file_reader(path) -> dict:
    """
    配置文件读取方法
    :param path: 文件路径
    :return: 配置字典
    """
    conf_file = open(path, 'r+')
    conf = conf_file.readlines()
    conf_dict = dict()
    for line in conf:
        if '=' in line:
            conf_name, conf_value = line.split('=')
            conf_dict[conf_name.replace(' ', '')] = conf_value.replace(' ', '').replace('\n','')
    conf_file.close()
    return conf_dict


def conf_file_writer(data_dict:dict, file:str) -> None:
    """
    配置文件写入方法
    :param data_dict: 配置字典
    :param file: 文件路径
    :return: None
    """
    file = open(file, "w+")
    for k in data_dict.keys():
        file.writelines("\n%s = %s" % (k, data_dict[k]))
    file.flush()
    file.close()


if __name__ == "__main__":
    image = open("test.jpg", 'rb')
    image = image.read()
    b64 = encode_bytes_data_b64(image)
    bytes = decode_b64_to_bytes(b64)
    image = decode_bytes_image(bytes)
    cv2.imshow("test", image)
    cv2.waitKey()