import pykafka
from threading import Thread
from pykafka.simpleconsumer import OffsetType

class Client:
    def __init__(self, host, port):
        '''
        init a new KafkaClient using given host and port
        @param host: host name
        @param port: server port
        '''
        self.Host = host
        self.Port = port
        self.Client = pykafka.KafkaClient("%s:%s" % (self.Host, self.Port))

    def sendMessage(self, topic, message):
        '''
        send a message to the given topic using default client
        if the client do not have the topic, an  error will raise
        '''
        if(self.Client.topics[topic] is None):
            raise("Can not find Such Topic:[%s]" % topic)
        t = self.Client.topics[topic]
        procuder = t.get_producer()
        procuder.produce(message.encode("UTF-8"))

    def StartListenMessage(self, topics, callback, group):
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
                    auto_offset_reset = pykafka.common.OffsetType.LATEST,
                    auto_commit_enable=True
                )
            runt = Thread(target=self.__onMessage__, args=(t, callback))
            runt.start()

    def __onMessage__(self, consumer, callback):
            while True:
                message = consumer.consume()
                callback(message)

if __name__ == "__main__":
    client = Client("ali.wwbweibo.me","9092")
    client.StartListenMessage(["test"], lambda msg: print(msg.value),'test')
    client.sendMessage("test", "test message from python")
