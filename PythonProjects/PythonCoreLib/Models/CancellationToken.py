class CancellationToken:
    def __init__(self):
        self.canceled = False

    def cancel_task(self):
        self.canceled = True

    def start_task(self):
        self.canceled = False

    def get_task_status(self):
        return self.canceled