### Abstract

  怎么知道web api被调用的频率了？  
  如何分析各时段web api各接口方法的调用次数？  
  如何获取web api接口方法的调用失败频次？  
  如何分析web api接口方法耗时异常？  

  Snake是基于C#开发的实时api监控平台，包括api压力监控和调用异常监控。  
  是基于RESTFul api风格开发的.net web api微服务应用集群，实时的接口调用监控为系统开发者提供了有利的分析数据。  
  Snake是为此而生的安静而平顺监控系统，它通过Filter实现监控嵌入，异步发送监控日志消息，通过基于RabbitMQ  
  实现的企业服务总线平顺处理消息，最终将监控日志数据输入存储到Mongodb中。  

### Requirements

  * Windows 7 、Windows Server 2008以上操作系统  
  * IIS 8.5 +  
  * .NET Framework 4.6  
  * [RabbitMQ 3.6.5](http://www.rabbitmq.com)  
  * [Mongodb 3.2.1](https://www.mongodb.com)  
  
### Quick Started

1. Snake.Api项目是webapi项目，编译并发布Snake.Api到IIS站点，修改配置文件web.config，如下：
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
2. Snake.ApiTrackService项目是windows服务，编译Snake.ApiTrackService项目
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
5. 在需要监控的webapi项目中引用Snake.Client.dll和Snake.Core.dll动态库，并增加配置：  
  * 在App_Start下WebApiConfig.cs文件中增加一行代码如下：  
  ```
	public static void Register(HttpConfiguration config)
	{
		// Web API 配置和服务            
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

### Copyright and license

[Apache 许可证2.0版](http://www.apache.org/licenses/LICENSE-2.0.html)

### AREAS FOR IMPROVEMENTS

QQ: 46313060  
Email: yepeng2002@icloud.com