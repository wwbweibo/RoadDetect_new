import torch.nn as nn

class Conv(nn.Module):
    """description of class"""
    def __init__(self, image_channel = 3):
        '''
        input size is 16 * 16 * image_channel
        '''
        super(Net, self).__init__()
        # make sure the have the same padding
        self.conv1_1 = nn.Conv2d(image_channel, 16, kernel_size=3, stride=1, padding=1)
        self.conv1_2 = nn.Conv2d(16, 16, kernel_size=3, stride=1, padding=1)
        self.max_pool1 = nn.MaxPool2d(kernel_size=2)
        # after shape is 8 * 8 * 16

        self.conv2_1 = nn.Conv2d(16, 32, kernel_size=3, stride=1, padding=1)
        self.conv2_2 = nn.Conv2d(32, 32, kernel_size=3, stride=1, padding=1)
        self.max_pool2 = nn.MaxPool2d(kernel_size=2)
        # output shape is 4 * 4 * 32

        self.fc1 = nn.Linear(16 * 32, 256)
        self.fc2 = nn.Linear(256, 512)
        self.model = nn.Sequential(
            self.conv1_1,
            self.conv1_2,
            self.max_pool1,
            self.conv2_1,
            self.conv2_2,
            self.max_pool2,
            self.fc1,
            self.fc2
            )

    def forward(self, x):
        return self.model(x)