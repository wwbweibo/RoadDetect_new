FROM openjdk
ADD kafka_2.12-2.4.0.tgz /
RUN mv /kafka_2.12-2.4.0 /kafka
WORKDIR /kafka
EXPOSE 9092
ENTRYPOINT "bin/kafka-server-start.sh" "config/server.properties"