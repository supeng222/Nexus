# 配置需求

## 需要部署的应用列表

+ API
+ Developer
+ CDN
+ OSS
+ Stargate
+ Account
+ Wiki
+ WWW

共计11一个应用。

其中：

    API, Developer, CDN, Account, Wiki, WWW, Kahla.Server, Kahla.App, Kahla.Home
这九个应用，对于每一个应用，如果要形成计算集群，在保证共用数据库的情况下，可以直接横向扩展。

    OSS

这一个应用，如果要形成计算集群，在保证共用数据库和共用存储空间的情况下，可以横向扩展。

    Stargate
这一个应用作为消息队列，它的内存中保存了大量数据，而无法进行横向扩展。

## 调试部署

### 基本硬件要求

| 项目        | 最低配置要求    |  推荐配置要求  |
|--|--|--|
|物理机数量|  0  |   1台全新的专用物理机
|处理器|   1 GHz 的64位处理器  |   3 GHz 的64位处理器
|内存|   4 GB 或更大  |   8GB 或更大
|硬盘空间|  40GB机械硬盘  |   200GB固态硬盘
|网络|  0.2Mb/s的下载速度  |   1Mb/s的下载速度
|域名资源|  不需要拥有域名  |   拥有一个域名
|IP地址|  不需要拥有公网IP  |   拥有一个公网IP
|操作系统|  Windows 7  |   Windows 10
|数据库|  SQL Server Express 2014 |   SQL Server Enterprise 2017

### 调试部署模型

+ 所有的应用都将部署在同一个服务器中
+ 不使用任何负载均衡设备
+ 存储服务将直接保存到本机硬盘
+ 不使用活动目录和域服务
+ 所有应用共享一个数据库，且数据库安装在本机
+ 所有应用不进行备份
+ 消息队列仅部署在本机

## 生产部署

| 项目        | 最低配置要求    |  推荐配置要求  |
|--|--|--|
|物理机数量|  20台企业级服务器  |   200台企业级服务器
|处理器|   每个处理器1.0GHz  |   每个处理器3.0GHz
|内存|   64 GB 或更大  |   1TB 或更大
|硬盘空间|  20块200GB机械硬盘  |   200块200GB的固态硬盘
|网络|  100M带宽  |   万兆网络和不限带宽
|域名资源|  一个域名  |   一个.com域名
|IP地址|  10个IP地址  |   100个IP地址
|操作系统|  Windows Server 2012 R2  |   Windows Server 2016数据中心版
|数据库|  SQL Server Enterprise 2017 |   SQL Server Enterprise 2017
|机柜|  100U的机柜 |   中央空调和至少1000U的企业级数据中心

### 生产部署模型

+ 每个可以横向扩展的应用运行在至少两台专用的服务器中
+ 为可以横向扩展的应用使用负载均衡设备以扩展其性能
+ 存储服务应用集群将存储内容放在专用共享存储设备上
+ 使用活动目录和域服务
+ 每个应用单独具有自己的专用数据库设备
+ 所有应用都进行备份，且使用专门的服务器进行备份
+ 消息队列专门部署在一台较高性能的服务器上

## 部署的平台

Aiursoft所依赖的全部框架、组件、包、数据库都是完全支持跨平台的。开发者可以在自己的Linux、MacOS、Windows上调试逐一微服务。无论开发者具有单台Windows Server或Linux，也都可以完整的将其部署在其中。但是，不幸的是，考虑到SQL Server和.NET Core部署的最佳实践是在Windows Server 2016上，暂不计划介绍Aiursoft微服务平台在Windows Server之外平台的部署方式。

同样的，所有上述内容也可以在容器（例如Docker）中完成。这里也不再给出官方的部署资料。请参考社区。