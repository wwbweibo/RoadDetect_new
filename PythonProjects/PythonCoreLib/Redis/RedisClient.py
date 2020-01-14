import redis

class RedisClient(object):
    def __init__(self, host, port):
        self.ConnectionPool = redis.ConnectionPool(host = host, port = port)
        self.client = redis.Redis(connection_pool = self.ConnectionPool)

    def set(self, key, value):
        self.client.set(key, value)

    def get(self, key):
        return self.client.get(key)

    def hset(self, name, k, v):
        self.client.hset(name, k, v)

    def hget(self, name, k):
        return self.client.hget(name, k)

    def lpush(self, name, value):
        self.client.lpush(name, value)

    def lpop(self, name, start=-1, end=-1):
        data = self.client.lpop(name)
        if 0 <= start <= end:
            return data[start:end]
        return data
