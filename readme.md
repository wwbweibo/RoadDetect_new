# 基于分布式和微服务的路面裂缝识别系统

该项目基于分布式和微服务搭建

## 项目基本的运行环境（基于docker）

base-env中包含了项目运行的基本环境，请首先在要使用的环境中启动这些容器。
启动容器请使用 `docker-compose up -d --build`
首次启动后务必对配置文件进行修改，保证zookeeper和kafka能够正常使用

## 微服务消息的基本规约

服务只监听自己感兴趣的消息，任何消息需要携带上消息的发送时间（已经在发送消息之间进行了统一的处理）
图像采集微服务：
	发送：
	TaskControl	任务的新建和结束
	TaskData	任务数据
	接受：
	ControlMessage	控制消息
图像计算服务：
	发送：
	TaskCalc	图像计算结果
	接受：
	ControlMesssage	控制消息
	TaskData
Master服务：
	接受：
	TaskControl
	TaskData
	TaskCalc
	发送:
	ControlMessage