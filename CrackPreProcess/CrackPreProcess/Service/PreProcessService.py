import cv2
from CrackPreProcess.Utils.Utils import Decodeb64String, DecodeByte2Image, EncodeData2b64
from CrackPreProcess.Kafka.Client import Client
import numpy as np

class PreProcessService:
    """description of class"""
    def __init__(self, base64string):
        self.imageb64String = base64string
        self.image = None
        self.client = None
        self.__decode_image__()
        
    def __decode_image__(self):
        bytedata = Decodeb64String(self.imageb64String)
        self.image = DecodeByte2Image(bytedata)

    def execute_workflow(self):
        if(self.image.shape[0] != self.image.shape[1] != 1024):
            raise Exception("input image shape error, require 1024 * 1024 image")
        self.cut_image()
        self.send_todo()

    def cut_image(self):
        im_list = []
        for i in range(64):
            for j in  range(64):
                im_list.append(self.image[i * 16: (i+1) * 16, j * 16: (j+1) * 16, :])
        # conver the image blocks to a numpy array 
        im_list = np.asarray(im_list, dtype=np.uint8)
        self.image_block = im_list
        self.serailized_image_block = im_list.tobytes()

    def convert_color_gray(self):
        """
        将bgr的图片转化为灰度图
        """
        self.gray_img = cv2.cvtColor(self.image, cv2.COLOR_BGR2GRAY)
    
    def resize_img(self, width=1024):
        """
        缩放图像大小，默认为1024
        @param width: 图像缩放至的宽度
        """
        self.image = cv2.resize(self.image,(width, int(width * img.shape[0] / img.shape[1])))
        # 检查灰度图的大小
        if self.gray_img is not None or self.gray_img.shape[0] != self.gray_img[1] != 1024:
            self.gray_image = cv2.resize(self.gray_image,(width, int(width * img.shape[0] / img.shape[1])))

    def resize_img1(self, width=1024, height=1024):
        self.image = cv2.resize(self.image, (width, height))
             

    def convert_color(self,image, code=cv2.COLOR_BGR2GRAY):
        """
        调整图像的色彩空间
        """
        return cv2.cvtColor(image, code)

    def center_avg_imp(img, ksize=10):
        """
        improve the image pixels by image center pixel average
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

    def equalize_hist(img, flag=False):
        """
        直方图均衡化
        """
        hist_img = np.zeros(shape=img.shape)
        hist_img = cv2.equalizeHist(img, hist_img)
        return hist_img


    def med_blur(img, ksize=3, flag=False):
        """
        对图像进行中值滤波
        """
        if img.dtype is not np.uint8:
            img = img.astype(np.uint8)

        new_img = cv2.medianBlur(img, ksize)
        return new_img


    def gauss_blur(img, ksize=[3, 3]):
        """
        对图像进行高斯模糊
        """
        cv2.GaussianBlur(img, ksize=ksize)


    def adj_gamma(img, flag=False):
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


    def binary_image(img, thresh=0.15, flag=False):
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

        ret, new_img = cv2.threshold(img, thresh, 255, cv.THRESH_BINARY)
        new_img = np.abs(new_img - 255)
        return new_img


    def hist_segmentation(img):
        """
        对图像进行直方图分割
        """
        hist = cv2.calcHist([img], [0], None, [256], [0, 255])
        max_index = np.where(hist == max(hist))
        mask = hist[0:max_index[0][0]]
        min_index = np.where(mask == min(mask))
        ret, new_im = cv2.threshold(img, min_index[0][0], 255, cv.THRESH_BINARY)
        return new_im


    def send_todo(self):
        if self.client is None:
            self.client = Client("ali.wwbweibo.me", "9092")
            self.client.sendMessage("calc-image", EncodeData2b64(self.serailized_image_block))

if __name__ == "__main__":
    from CrackPreProcess.Utils import Utils
    file = open("D:\\OneDrive\\Pictures\\47526064_p0.png.jpg", 'rb').read()
    b64string = Utils.EncodeData2b64(file)
    service = PreProcessService(b64string)
    service.resize_img1()
    service.execute_workflow()