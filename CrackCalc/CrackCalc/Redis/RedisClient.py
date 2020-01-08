import redis

class RedisClient(object):
    def __init__(self, host, port):
        self.ConnectionPool = redis.ConnectionPool(host = host, port = port)
        self.client = redis.Redis(connection_pool = self.ConnectionPool)

    def set(self, key, value):
        self.client.set(key, value)

    def get(self, key):
        return self.client.get(key)
        

