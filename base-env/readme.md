# 项目基本的运行环境（基于docker）

该目录中包含了项目运行的基本环境，请首先在要使用的环境中启动这些容器。
启动容器请使用 `docker-compose run -d --build`
首次启动后务必对配置文件进行修改，保证zookeeper和kafka能够正常使用