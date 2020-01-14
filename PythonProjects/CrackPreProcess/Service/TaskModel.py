import json

class TaskModel:
    """
    this  class include the basic  task info
    """
    def __init__(self):
        self.TaskSource = None
        self.TaskCreateTime = None
        self.TaskId = None
        self.TaskDetail = None

class TaskDetail:
    def __init__(self):
        self.Id = None
        self.Image = None
        self.b64Image = None
        self.ImageBlocks = None
        self.ImageBlocksBytes = None
        self.ImageBlocksb64Data = None