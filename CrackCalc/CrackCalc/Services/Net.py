import torch.nn as nn

class Net(object):
    """description of class"""
    def __init__(self, image_channel = 3):
        super(Net, self).__init__()
        self.conv1_1 = nn.Conv2d(image_channel, 32, kernel_size=3)
        self.conv1_2 = nn.Conv2d(32, )

