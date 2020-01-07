from kazoo.client import KazooClient, KazooState

class ZkClient(object):
    def __init__(self, hosts, ports):
        server = ''
        for host,port in zip(hosts, ports):
            server = server + '%s:%s,' % (host, port)
        server = server[:len(server) - 2]
        self.zk = KazooClient(server)
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

    def register_service(self, service_name, service_type):
        '''
        用于向zookeeper注册远程服务
        使用一个临时节点表示一个特定的服务实例
        '''
        path = '/%s/%s' % (service_type, service_name)
        self.zk.create(path,None,ephemeral=True, makepath=True)

    def create_task(self, task_type, task_id):
        todopath = "/%s/todo/%s" % (task_type, task_id)
        self.zk.create(todopath)

    def require_task(self, task_type, task_id, service_id):
        '''
        向zk请求一个任务
        首先向特定的任务类型节点下请求一个临时节点，如果没有报错，则表示任务请求成功，否则任务请求失败
        service_id指定了执行任务的服务
        '''
        # 节点路径 /{tasktype}/{inprogress}/{taskid}
        todopath = "/%s/todo/%s" % (task_type, task_id)
        path = "/%s/inprogress/%s" % (task_type, task_id)
        try:
            # 首先确认待办中存在对应的节点
            if self.zk.exists(todopath):
                self.zk.create(path, service_id, ephemeral=True)
                return True
            # 任务节点不存在，则任务已经被执行了
            else:
                return False
        except:
            # 出现异常，任务请求失败
            return False

    def finish_task(self, task_type, task_id):
        todopath = "/%s/todo/%s" % (task_type, task_id)
        path = "/%s/inprogress/%s" % (task_type, task_id)
        self.zk.delete(todopath)
        self.zk.delete(path)
        