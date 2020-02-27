import cv2
from PythonCoreLib.Utils.Utils import decode_b64_to_bytes, decode_bytes_image, encode_bytes_data_b64, decode_bytes_numpy_array
from PythonCoreLib.Redis.RedisClient import RedisClient
import numpy as np
import uuid
import time
import json

class PreProcessService:
    """description of class"""
    def __init__(self, conf):
        self.conf = conf
        self.redis = RedisClient(conf['redis_host'], conf['redis_port'])

    def __decode_image__(self, b64image):
        image = decode_b64_to_bytes(b64image)
        image = decode_bytes_image(image)
        return image

    def __load_data__(self, task, datatype):
        '''
        从内存或redis中读取数据
        '''
        if datatype == 'taskId':
            if task is None:
                raise Exception("The given task is error or empty, task:" % task)
            imageb64 = self.redis.hget(task.majorTaskId, task.subTaskId)
            if imageb64 is None:
                raise Exception("trying to ge image error")
            image = self.__decode_image__(imageb64)
        else:
            image = self.__decode_image__(task)
        return image

    def execute_workflow(self, task, datatype='taskId'):
        '''
        执行预处理工作流
        task：任务Id或者任务数据
        @param datatype: taskId->输入的数据是任务ID，taskData->输入的是任务数据
        @return: 切分后的图像
        '''
        image = self.__load_data__(task, datatype)
        if(image.shape[0] != image.shape[1] and image.shape[0] != 1024):
            image = self.resize_img1(image)
        if len(image.shape) == 3:
            image = self.convert_color_gray(image)
        image_block, serailized_image_block = self.cut_image(image)
        return image_block, image

    def cut_image(self, image):
        '''
        图像分割
        :param image: 输入图像
        :return:
        '''
        im_list = []
        for i in range(64):
            for j in  range(64):
                im_list.append(image[i * 16: (i+1) * 16, j * 16: (j+1) * 16])
        # conver the image blocks to a numpy array 
        im_list = np.asarray(im_list, dtype=np.uint8)
        im_list = im_list.reshape((64*64, 16,16,1))
        image_block = im_list
        serailized_image_block = im_list.tobytes()
        return image_block, serailized_image_block

    def convert_color_gray(self, image):
        """
        将bgr的图片转化为灰度图
        """
        return cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

    def resize_img1(self, image,width=1024, height=1024):
        return cv2.resize(image, (width, height))

    def center_avg_imp(self, img, ksize=10):
        """
        提升图像质量保证图像各部分亮度一致
        """
        new_img = np.copy(img)

        dw = int(img.shape[1] / 7)
        dh = int(img.shape[0] / 7)

        region_1 = new_img[dh * 1: dh * 2, dw * 1: dw * 2]
        region_2 = new_img[dh * 1: dh * 2, dw * 5: dw * 6]
        region_3 = new_img[dh * 5: dh * 6, dw * 5: dw * 6]
        region_4 = new_img[dh * 1: dh * 2, dw * 5: dw * 6]
        region_5 = new_img[dh * 3: dh * 4, dw * 3: dw * 4]

        avg1 = np.average(region_1)
        avg2 = np.average(region_2)
        avg3 = np.average(region_3)
        avg4 = np.average(region_4)
        avg5 = np.average(region_5)

        avg = (avg1 + avg2 + avg3 + avg4 + avg5) / 5

        for x in range(0, img.shape[0], ksize):
            for y in range(0, img.shape[1], ksize):
                new_img[x:x + ksize, y:y + ksize] = \
                    img[x:x + ksize, y:y + ksize] * (avg / np.average(img[x:x + ksize, y:y + ksize]))
        return new_img

    def equalize_hist(self, img, flag=False):
        """
        直方图均衡化
        """
        hist_img = np.zeros(shape=img.shape)
        hist_img = cv2.equalizeHist(img, hist_img)
        return hist_img


    def med_blur(self, img, ksize=3, flag=False):
        """
        对图像进行中值滤波
        """
        if img.dtype is not np.uint8:
            img = img.astype(np.uint8)

        new_img = cv2.medianBlur(img, ksize)
        return new_img


    def gauss_blur(self, img, ksize=[3, 3]):
        """
        对图像进行高斯模糊
        """
        cv2.GaussianBlur(img, ksize=ksize)


    def adj_gamma(self, img, flag=False):
        """
        对图像进行归一化处理
        :param img: 输入图像
        :param flag: 是否显示归一化之后的图像
        :return: 归一化之后的图像
        """
        new_image = img
        new_image = new_image - np.min(np.min(new_image))
        new_image = new_image / np.max(np.max(new_image))

        return new_image


    def binary_image(self, img, thresh=0.15, flag=False):
        """
        对图像进行二值化
        :param img: 输入图形
        :param thresh: 阈值
        :param flag: 是否显示结果
        :return: 二值化之后的图形
        """
        t = np.reshape(img, img.shape[1] * img.shape[0])
        pixel = np.bincount(t)
        xdata = np.linspace(1, pixel.shape[0], pixel.shape[0])
        index = np.argwhere(pixel == np.max(pixel))
        thresh = index[0][0] / 3

        ret, new_img = cv2.threshold(img, thresh, 255, cv2.THRESH_BINARY)
        new_img = np.abs(new_img - 255)
        return new_img


    def hist_segmentation(self, img):
        """
        对图像进行直方图分割
        """
        hist = cv2.calcHist([img], [0], None, [256], [0, 255])
        max_index = np.where(hist == max(hist))
        mask = hist[0:max_index[0][0]]
        min_index = np.where(mask == min(mask))
        ret, new_im = cv2.threshold(img, min_index[0][0], 255, cv2.THRESH_BINARY)
        return new_im