# 基于分布式和微服务的路面裂缝识别系统

该项目基于分布式和微服务搭建

## 项目基本的运行环境（基于docker）

base-env中包含了项目运行的基本环境，请首先在要使用的环境中启动这些容器。
启动容器请使用 `docker-compose up -d --build`
首次启动后务必对配置文件进行修改，保证zookeeper和kafka能够正常使用

## 微服务消息的基本规约

服务只监听自己感兴趣的消息，任何消息需要携带上消息的发送时间（已经在发送消息之间进行了统一的处理）
在消息发送和接受时一致使用枚举值。

```protobuf
消息枚举定义
enum MessageTopicEnum{
	// 任务控制消息
	TaskControl = 0;
	// 任务数据消息
	TaskItemData = 1;
	// 任务计算消息
	TaskCalc = 2;
	// 服务控制消息
	ControlMessage = 4;
}
```

图像采集微服务：
	发送：
	TaskControl	任务的新建和结束
	TaskItemData	任务数据
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

## 服务名称定义

服务名称方式统一为 {ServiceType}-{ServiceId}
ServiceType:
	DataCollect		数据采集服务
	DataCalc		数据计算服务
	MasterService	Master主服务

```
服务类型枚举定义
enum ServiceType{
	MasterService=0;
	DataCollect=1;
	DataCalc=2;
}
```

## 服务发现&任务发布规则

任何服务在启动时必须在Zookeeper对应的服务节点下创建临时节点，节点内容定义为当前服务的状态。
数据采集服务在发布任何任务相关消息之间必须在zookeeper任务节点下创建对应的待办任务节点
数据计算服务在接收到消息之后需要对任务进行抢占，方式为在zookeeper任务节点下创建对应的处理中任务节点，创建成功表示任务抢占成功，才可以进行数据的计算。数据计算完成之后应当删除对应的代办和正在处理的任务节点。
在计算任务处理中出现异常，应当删除正在处理任务节点，释放任务。

### 数据存储规约

为了减少数据库压力，图像数据的写入只有在任务计算完成之后进行，再次之前任务保存在redis缓存的hashset中，key为任务id，name为数据id
对于基本任务信息，Master服务通过监听对应的任务控制消息向数据库中写入任务的基本信息。监听任务数据消息向数据库写入基本的任务数据消息，监听数据计算消息更新任务数据消息，并从缓存中读取出任务的图像数据并保存到数据库。
	
### Zookeeper节点定义

	/----							zookeeper根节点
		|----CrackDetect			项目主节点
			|----Service			Service父节点
				|----1				数据采集服务父节点
					|----ServiceId-1
					|----ServiceId-2
				|----2				数据计算服务父节点
				|----0				Master服务父节点
				|----Task				任务父节点
					|----todo			代办任务父节点
						|----MajorTaskId
							|----SubTaskId
					|----doing			正在处理的任务父节点