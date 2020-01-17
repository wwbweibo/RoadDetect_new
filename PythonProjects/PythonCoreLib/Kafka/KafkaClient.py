import pykafka
from threading import Thread
from pykafka.simpleconsumer import OffsetType
import json
import time


class KafkaClient:
    def __init__(self, host, port):
        '''
        init a new KafkaClient using given host and port
        @param host: host name
        @param port: server port
        '''
        self.Host = host
        self.Port = port
        self.Client = pykafka.KafkaClient("%s:%s" % (self.Host, self.Port))
        self.RunningThread = None

    def send_message(self, topic, message):
        '''
        send a message to the given topic using default client
        if the client do not have the topic, an  error will raise
        '''
        if(self.Client.topics[topic] is None):
            raise("Can not find Such Topic:[%s]" % topic)
        t = self.Client.topics[topic]
        message_send = dict()
        message_send = {'data':message, 'CreateTime':int(time.time())}
        procuder = t.get_producer()
        procuder.produce(json.dumps(message_send).encode("UTF-8"))

    def start_listen_message(self, topics, callback, group):
        '''
        start to listen message from topics when message arrived the callback will be called
        '''
        tList = []
        for topic in topics:
            if self.Client.topics[topic] is not None:
                tList.append(self.Client.topics[topic])
            else:
                raise("Can not find Such Topic:[%s]" % topic)
        for t in tList:
            t = t.get_simple_consumer(
                    consumer_group=group,
                    auto_offset_reset=OffsetType.LATEST,
                    auto_commit_enable=True
                )
            runt = Thread(target=self.__on_message__, args=(t, callback))
            self.RunningThread = runt
            self.RunningThread.start()

    def __on_message__(self, consumer, callback):
            while True:
                message = consumer.consume()
                try:
                    data = json.loads(message.value.decode('UTF-8'))
                    if data['CreateTime'] > (int(time.time()) - 10): # 只处理十秒内的消息
                        taskThread = Thread(target=callback, args=(data['data'],))
                        taskThread.start()
                except:
                    pass

    def stop_listen(self):
        if self.RunningThread is not None:
            self.RunningThread.setDaemon(True)


if __name__ == "__main__":
    client = KafkaClient("ali.wwbweibo.me","9092")
    client.start_listen_message(["test"], lambda msg: print(msg.value),'test')
    client.send_message("test", "test message from python")
