### Abstract

  如何分析web api被调用的频率？  
  如何分析web api接口方法耗时？  
  如何分析分布式应用日志？  

  Snake是基于C#开发的日志中心，实现了应用程序日志采集平台和实时api监控平台，包括api压力监控和调用监控。  
  是基于RESTFul api风格开发的.net web api服务应用集群，实时的接口调用监控为系统开发者提供了有利的分析数据。  
  Snake是为此而生的安静而平顺监控系统，它通过Filter实现监控嵌入，异步发送监控日志消息，通过基于RabbitMQ  
  实现的消息总线平顺处理消息，最终将监控日志数据输入存储到Mongodb中。  

### 序

  Snake日志平台目前推出了V1.0，该版本实现了日志的采集，存储和分析，采集端可支持web api服务通过过滤器自动采集和应用主动发送应用日志请求，
  存储端实现了日志存储和应用日志应用名和标签解析，分析端实现应用日志条件查询和日志消息合并，后续还需完善api监控日志的分析和服务接口调用路径树结构解析。

### Requirements

  * Windows 7 、Windows Server 2008以上操作系统  
  * IIS 8.5 +  
  * .NET Framework 4.6  
  * [RabbitMQ 3.6.5](http://www.rabbitmq.com)  
  * [Mongodb 3.2.1](https://www.mongodb.com)  
  * [redis-2.4.5](https://redis.io/)
  
### Snake架构

<img src="https://github.com/yepeng2002/Snake/blob/master/resource/images/SnakeLogic.png" alt="架构" />

### Quick Started

1. Snake.Api项目是webapi项目，负责发送日志请求的响应和查询日志接口，作为生产者将日志发布到消息队列，作为查询接口从Mongodb提取日志数据，编译并发布Snake.Api到IIS站点，修改配置文件web.config，如下：
  * RabbitMQ配置
```
	<!--************************* RabbitMQ Connection Settings ****************** -->
	<RabbitMQ.Connection>
		<add key="RabbitMQ.HostName" value="localhost" />
		<add key="RabbitMQ.Port" value="5672" />
		<add key="RabbitMQ.UserName" value="snake" />
		<add key="RabbitMQ.Password" value="snake" />
		<add key="RabbitMQ.VirtualHost" value="snakeTest" />
		<add key="RabbitMQ.QueueName" value="q_snake_apitrack" />
		<!--重试次数-->
		<add key="RabbitMQ.UseRetryNum" value="3" />
	</RabbitMQ.Connection>
```
  * Mongo配置
```
    <add key="MongoConnectionString" value="mongodb://snake:snake@localhost:27017/SnakeDbTest/?MaximumPoolSize=500;socketTimeoutMS=2000;MinimumPoolSize=1;waitQueueTimeoutMS=300;waitQueueMultiple=10;ConnectionLifetime=30000;ConnectTimeout=30000;Pooled=true" />
```
  * Redis配置
```
  <!--Redis配置参数,格式：password@IP:port-->
  <RedisConfig WriteServerList="123456@127.0.0.1:6379" ReadServerList="123456@127.0.0.1:6379" MaxWritePoolSize="60" MaxReadPoolSize="60" AutoStart="true" DefaultDb="3" LocalCacheTime="180" RecordeLog="false">
  </RedisConfig>
```
2. Snake.ApiTrackService项目是windows服务，是消息的消费者，并将消费的消息存入MongoDB，同时解析应用和标签存入缓存服务Redis供Snake.Api提取Applications和tags给Snake.App客户端选择，编译Snake.ApiTrackService项目
  * 修改配置文件App.config配置项，如下：
```
	<appSettings>
		<add key="MongoConnectionString" value="mongodb://snake:snake@localhost:27017/SnakeDbTest/?MaximumPoolSize=500;socketTimeoutMS=2000;MinimumPoolSize=1;waitQueueTimeoutMS=300;waitQueueMultiple=10;ConnectionLifetime=30000;ConnectTimeout=30000;Pooled=true" />
	</appSettings>  
	<!--************************** snake.Service Settings ********************** -->
	<snake.service>
		<add key="snake.serviceName" value="SnakeConsumer" />
		<add key="snake.serviceDisplayName" value="Snake Consumer Server" />
		<add key="snake.serviceDescription" value="Snake Consumer Server" />
	</snake.service>
	<!--************************* RabbitMQ Connection Settings ****************** -->
	<RabbitMQ.Connection>
		<add key="RabbitMQ.HostName" value="localhost" />
		<add key="RabbitMQ.Port" value="5672" />
		<add key="RabbitMQ.UserName" value="snake" />
		<add key="RabbitMQ.Password" value="snake" />
		<add key="RabbitMQ.VirtualHost" value="snakeTest" />
		<add key="RabbitMQ.QueueName" value="q_snake_apitrack" />
		<!--`消费者个数-->
		<add key="RabbitMQ.ConsumerNum" value="4" />
		<!--重试次数-->
		<add key="RabbitMQ.UseRetryNum" value="3" />
	</RabbitMQ.Connection>
```
  * 执行start.bat批处理脚本，安装或更新windows服务  
3. 安装RabbitMQ，根据上述1和2的配置设置用户密码和VirtualHost  
4. 安装Mongodb，根据上述2的配置设置用户密码和数据库，执行script目录下的MongoScript.js脚本  
5. 在需要监控的webapi项目（项目Snake.DemoApi中有使用示例代码）中引用Snake.Client.dll和Snake.Core.dll动态库，并增加配置：  
  * 在App_Start下WebApiConfig.cs文件中增加一行代码如下：  
  ```
	public static void Register(HttpConfiguration config)
	{
            // Web API 配置和服务
            string filterEnabled = ConfigurationManager.AppSettings["TrackLogFilterEnabled"];
            if (!string.IsNullOrEmpty(filterEnabled) && filterEnabled.ToLower() == "true")
                config.Filters.Add(new TrackLogActionFilterAttribute());  //api执行事件跟踪日志
  ```
  * 在web.config配置文件中增加配置如下：  
```
	<appSettings>
		<!--TrackLog过滤器开关-->
		<add key="TrackLogFilterEnabled" value="true" />
		<!--SnakeApi服务 验签-->
		<add key="SnakeApi" value="SNAKE_API" />
		<add key="SnakeApiSecret" value="1!2@3#4$5" />
		<!--Snake.Api服务 接口地址-->
		<add key="SnakeServerApi" value="http://localhost:50424" />
	</appSettings>
```
6. 安装Redis
7. Snake.App项目是日志分析应用的客户端程序，支持分页加载日志数据，支持日志合并
  * App.config配置文件中增加配置如下：  
```
  <appSettings>
    <!--SnakeApi服务 验签-->
    <add key="SnakeApi" value="SNAKE_API" />
    <add key="SnakeApiSecret" value="1!2@3#4$5" />
    <!--Snake.Api服务 接口地址-->
    <add key="SnakeServerApi" value="http://localhost:50424" />
  </appSettings>
```
8. 应用日志发送，引用Snake.Client.dll和Snake.Core.dll动态库，并增加配置，如下：
  * 示例项目Snake.DemoConsole配置文件中增加配置如下：  
```
  <appSettings>
    <!--SnakeApi服务 验签-->
    <add key="SnakeApi" value="SNAKE_API" />
    <add key="SnakeApiSecret" value="1!2@3#4$5" />
    <!--Snake.Api服务 接口地址-->
    <add key="SnakeServerApi" value="http://localhost:50424" />
  </appSettings>
```
  * 应用日志示例代码：  
```
	LogProxy.Error("Exception : ", "Snake.DemoConsole", 4, new List<string>() { "Block", "Red" });
	LogProxy.Debug("Debug : ", "Snake.DemoConsole", tags: new List<string>() { "Blue", "Red" });
```
### Snake.App界面

<img src="https://github.com/yepeng2002/Snake/blob/master/resource/images/SnakeAppLog.png" alt="应用日志分析" />


### Copyright and license

[Apache 许可证2.0版](http://www.apache.org/licenses/LICENSE-2.0.html)

### AREAS FOR IMPROVEMENTS

QQ: 46313060  
Email: yepeng2002@sina.com