import cv2
from CrackPreProcess.Utils.Utils import Decodeb64String, DecodeByte2Image
from CrackPreProcess.Kafka.Client import Client
import numpy as np

'''
流程待定
'''
class PreProcessService(object):
    """description of class"""
    def __init__(self, base64string):
        self.imageb64String = base64string
        self.image = None
        self.__decode_image__()
        
    def __decode_image__(self):
        bytedata = Decodeb64String(self.imageb64String)
        self.image = DecodeByte2Image(bytedata)
        if(self.image.shape[0] != self.image.shape[1] != 1024):
            raise("input image shape error, require 1024 * 1024 image")

    def ExecuteWorkFlow(self):
        return self.image.shape

    def CutImage(self):
        im_list = []
        for i in range(64):
            for j in  range(64):
                im_list.append(self.image[i * 16: (i+1) * 16, j * 16: (j+1) * 16, :])
        # conver the image blocks to a numpy array 
        
        self.image_block = im_list

    def convert_color_gray(self):
        """
        将bgr的图片转化为灰度图
        :type image: opencv image
        :param image: the image need to convert
        :return: an image in gray color
        """
        self.gray_img = cv2.cvtColor(self.image, cv2.COLOR_BGR2GRAY)
    
    def resize_img(self, width=1024):
        """
        缩放图像大小，默认为1024
        :type img: image
        :param img: input image
        :type width: int
        :param width: width after resize,800 as default
        :return: image after resize
        """
        self.image = cv2.resize(self.image,(width, int(width * img.shape[0] / img.shape[1])))
        # 检查灰度图的大小
        if self.gray_img is not None or self.gray_img.shape[0] != self.gray_img[1] != 1024:
            self.gray_image = cv2.resize(self.gray_image,(width, int(width * img.shape[0] / img.shape[1])))

    def convert_color(self,image, code=cv.COLOR_BGR2GRAY):
        """
        convert color space of an image
        :type image: image
        :param image: input image
        :type code: opencv convert code
        :param code: opencv color convert , COLOR_BGR2GRAY as default
        :return: image after convert
        """
        return cv.cvtColor(image, code)

    def center_avg_imp(img, ksize=10, flag=False):
        """
        improve the image pixels by image center pixel average
        :type img: image
        :param img: the image need to be improved
        :type ksize: int
        :param ksize: the filter size, 10 as default
        :type flag: Boolean
        :param flag: show the result or not
        :return: the result after deal
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
        # img = cv.medianBlur(img, 3)
        if flag:
            plt.subplot(1, 2, 1)
            plt.imshow(img, cmap='gray')
            plt.subplot(1, 2, 2)
            plt.imshow(new_img, cmap='gray')
            plt.show()

        return new_img

    def equalize_hist(img, flag=False):
        """
        equalize hist to improve image
        :type img: image
        :param img: input image
        :type flag: Boolean
        :param flag: show the result if is True, False as default
        :return: the image after equalize hist
        """
        hist_img = np.zeros(shape=img.shape)
        hist_img = cv.equalizeHist(img, hist_img)
        if flag:
            plt.subplot(2, 2, 1)
            plt.imshow(img, cmap="gray")
            plt.title("原图")
            plt.subplot(2, 2, 2)
            plt.hist(img)
            plt.title("原图直方图")
            plt.subplot(2, 2, 3)
            plt.imshow(hist_img, cmap="gray")
            plt.title("均衡化结果")
            plt.subplot(2, 2, 4)
            plt.hist(hist_img)
            plt.title("均衡化结果直方图")
            plt.show()
        return hist_img


def med_blur(img, ksize=3, flag=False):
    """
    对图像进行中值滤波
    :param img: input image
    :param ksize: size of filter
    :return: image after median filter
    """

    if img.dtype is not np.uint8:
        img = img.astype(np.uint8)

    new_img = cv.medianBlur(img, ksize)
    if flag:
        plt.subplot(2, 2, 1)
        plt.imshow(img, cmap="gray")
        plt.title("原图")
        plt.subplot(2, 2, 2)
        plt.hist(img)
        plt.title("原图直方图")
        plt.subplot(2, 2, 3)
        plt.imshow(new_img, cmap="gray")
        plt.title("中值滤波结果")
        plt.subplot(2, 2, 4)
        plt.hist(new_img)
        plt.title("中值滤波结果直方图")
        plt.show()
    return new_img


    def gauss_blur(img, ksize=[3, 3]):
        """
        对图像进行高斯模糊
        """
        cv.GaussianBlur(img, ksize=ksize)


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

        if flag:
            x = np.arange(0, new_image.shape[1], 1)
            y = np.arange(0, new_image.shape[0], 1)
            xg, yg = np.meshgrid(x, y)
            fig = plt.figure()
            ax = Axes3D(fig)
            ax.plot_surface(xg, yg, new_image, rstride=1, cstride=1, cmap=cm.viridis)
            plt.show()

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
        plt.plot(pixel)
        plt.show()

        ret, new_img = cv.threshold(img, thresh, 255, cv.THRESH_BINARY)
        new_img = np.abs(new_img - 255)
        if flag:
            plt.subplot(2, 1, 1)
            plt.imshow(img, cmap="gray")
            plt.subplot(2, 1, 2)
            plt.imshow(new_img, cmap="gray")
            plt.show()
        return new_img


    def hist_segmentation(img):
        """
        对图像进行直方图分割
        """
        hist = cv.calcHist([img], [0], None, [256], [0, 255])
        max_index = np.where(hist == max(hist))
        mask = hist[0:max_index[0][0]]
        min_index = np.where(mask == min(mask))
        ret, new_im = cv.threshold(img, min_index[0][0], 255, cv.THRESH_BINARY)
        return new_im


    def sendTodo(self):
        client = Client("ali.wwbweibo.me", "9092")

        client.sendMessage("calc-image", )



