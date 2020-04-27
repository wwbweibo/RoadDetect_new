from kazoo.client import KazooClient, KazooState
import PythonCoreLib.Models.ConstData as ConstData
import PythonCoreLib.Models.ServiceStatusEnum_pb2 as ServiceStatusEnum

class ZkClient:
    def __init__(self, hosts, ports, on_failure_action=None):
        server = ''
        for host, port in zip(hosts, ports):
            server = server + '%s:%s,' % (host, port)

        server = server[:len(server) - 1]
        self.zk = KazooClient(server)
        if on_failure_action is not None:
            if on_failure_action == 'RECONN':
                self.zk.add_listener(self.__zk_status_listener__)
        self.__service_node = ""
        self.zk.start()

    def get_children(self, node):
        if self.zk.exists(node):
            return self.zk.get_children(node)

    def create_node(self, node_name, data):
        self.zk.ensure_path(node_name)

    def update_node(self, node_name, data):
        self.zk.set(node_name, data)

    def delete_node(self, node_name):
        self.zk.delete(node_name)

    def read_data(self, node):
        if self.zk.exists(node):
            return self.zk.get(node)

    def register_service(self, service_type, service_id):
        """
        向Zookeeper注册远程服务
        :param service_type:    服务类型
        :param service_id:      服务ID
        :return:
        """
        path = ConstData.ServicePath % (service_type, service_id)
        self.__service_node = path
        self.zk.create(path, str(ServiceStatusEnum.Idle).encode("UTF-8"), ephemeral=True, makepath=True)

    def create_task(self, major_task_id, sub_task_id):
        todopath = ConstData.TodoTaskPath % (major_task_id, sub_task_id)
        self.zk.create(todopath, makepath=True)

    def require_task(self, major_task_id, sub_task_id,service_id) -> bool:
        '''
        向zk请求一个任务
        首先向特定的任务类型节点下请求一个临时节点，如果没有报错，则表示任务请求成功，否则任务请求失败
        service_id指定了执行任务的服务
        '''
        todo_path = ConstData.TodoTaskPath % (major_task_id, sub_task_id)
        path = ConstData.InProgressPath % (major_task_id, sub_task_id)
        result = False
        try:
            # 首先确认待办中存在对应的节点
            if self.zk.exists(todo_path):
                self.zk.create(path, service_id.encode('UTF-8'), ephemeral=True, makepath=True)
                try:
                    self.zk.set(self.__service_node, str(ServiceStatusEnum.Running).encode("UTF-8"))
                    result = True
                except:
                    # 如果更改状态出错，应该放弃该任务
                    self.zk.delete(path)
                    result = False
            # 任务节点不存在，则任务已经被执行了
            else:
                result = False
        except Exception as e:
            print(e)
            # 出现异常，任务请求失败
            result = False
        return result

    def finish_task(self, major_task_id, task_id):
        todo_path = ConstData.TodoTaskPath % (major_task_id, task_id)
        path = ConstData.InProgressPath % (major_task_id, task_id)
        self.zk.delete(todo_path)
        self.zk.delete(path)

    def task_execute_error(self, task_type, task_id):
        path = ConstData.InProgressPath % (task_type, task_id)
        self.zk.delete(path)

    def __zk_status_listener__(self, state):
        if state == KazooState.LOST:
            self.zk.start()

    def start_service(self):
        self.zk.set(self.__service_node, str(ServiceStatusEnum.Idle).encode("utf-8"))

    def running_service(self):
        self.zk.set(self.__service_node, str(ServiceStatusEnum.Running).encode("utf-8"))

    def stop_service(self):
        # 停止服务其实只是把节点状态设置为了服务下线
        self.zk.set(self.__service_node, str(ServiceStatusEnum.Offline).encode("utf-8"))
    def idle_service(self):
        self.start_service()

if __name__ == "__main__":
    from threading import Thread
    def test():
        print(client.require_task("testTask", "1", "1"))
    client = ZkClient(['localhost'], ['2181'])
    th1 = Thread(target=test)
    th2 = Thread(target=test)
    th3 = Thread(target=test)
    th1.start()
    th2.start()
    th3.start()
