# <img src="https://raw.githubusercontent.com/idealshol/picture/main/imgs1202501040323753.jpg" style="zoom:50%;" />

# 用途 2025.1

ROS类似一个数据传输的管道,也提供接口,好比cpu和其他设备间信息交互的总线.一开始是为解决机器人上信息传输问题,因而目前ROS开发社区中对这一块的库可以说很丰富了(包括处理各类传感器信息,控制命令,节点间的通信)
比如,在同台电脑同一操作系统上,一边用仿真软件做机械臂仿真(这里就当用模型是URDF或axcro一类)那我可以写一段ROS程序来获取仿真中机械臂的运动,同时写发布函数,再开个matlab,下载相应库,再写段接收函数,就能获取
机械臂的运动数据,同时在matlab上进行反运动学处理(IK)获取每个电机需要转动角.

# ROS1和ROS2

ROS每一段时间(4year?)会更新一次版本,同时官方对每个版本也有一定维护时期,目前(2023.9.22)[^ROS1]算是已经到头了.

## 使用

(越成熟的,上手越顺滑)

* ROS1只适配linux系统   || ROS2可以跨平台,支持win,linux,mac

* ROS1针对C++和Python的库为roscpp和rospy || ROS2针对C++和Python的库为rclpp和rclpy

* ROS1中的节点和消息由一个Master管理,需要在终端中运行 **roscore** 来启动,后续才可以运行节点和通信 || ROS2采用**DDS**作为中间层,内部有用c语言实现的rmw库来和DDS直接交互

* ROS1中用**catkin tool**来编译包 || ROS2中用**colcon tool**来做编译

* ROS1中python文件运行相对方便,调试也轻松不用重新编译 || ROS2中python程序运行需要编写setup.py文件来指定入口,且修改程序需要重新编译  (待修正)

* ROS1的Launch编写只限与xml格式,功能上也比较简单,启动文件,加载参数 || ROS2的Launch可采取*py*,*xml*,*yaml*格式 (**其实还是建议用.py,其他的我用的不是很上**) ROS2启动gazebo需要重新安装(ros1的在这不顶用,包括其他的一些包[urdf...],都存在ros1,ros2不共用问题)

* ``sudo apt install ros-foxy(ros2版本)-gazebo-ros-pkgs``

* ROS1和ROS2可以共存,但是不能同时在环境变量中启用

* ROS1配置工作空间 **source /devel/setup.bash/** || ROS2配置工作空间 **. install/setup.bash**   (也就是刷新环境,如果将此代码放置在环境变量 *~/.bashrc* 中则不用每次使用前都输入)

* ROS1的导航(navigation)可通过 *move_base* 包来实现 || ROS2的导航需要通过 *Navigation2(nav2)* 来实现.

  ``echo “source ~/work_space(工作空间)/devel/setup.bash” >> ~/.bashrc``

### setup.py打包

c++程序的项目依赖和信息可以从CMakelist.tex和package.xml里获取. 在 *ROS2* 的python项目中主要依赖setup.py和package.xml. setup.py负责对项目打包,在ROS2中用来简化python项目打包过程,同时也能从package.xml里动态导入信息到python安装文件中,将硬编码信息更改为调用 Ros2Setup 以从 XML 文件中提取数据.

C++项目中:package.xml包含了项目所需软件包的元信息,CMakelist.txt来描述项目软件包如何去代码构建(所需要编译内容). Python项目中没有CMakelist.txt,取而代之由 *setup.py* 来描述如何安装软件包
除此之外,完整的ROS2 Python项目还可能具有 *setup.cfg*(存放功能包中可执行文件) 和一个在功能包中与功能包同名的文件(用于存放__init__.py,ros2通过这个来找寻功能包).        !!!!!!!!!!!!!!!!!!!!!

# ROS1和ROS2的一些基础信息

1. ROS作为数据传输工具,具有3种传输方式[topic,service,action],有一个类似的[parameter.service].
   **Topic:** 是一类数据 *同步* 传输方案,节点间通过publisher和subscriber进行数据收发.
   **Service:** 是一类数据 *异步* 传输方案,节点间通过server和client进行数据收发.
   **Action:** 是一类数据 *指定* 传输方案,通过按一定频率发布数据,同时接收返回值(若中间没有其他传输干扰)一直执行到达成目标,节点间通过(Action)server和(Action)client.

2. [^launch]文件类似于bash脚本,可以一件执行多个文件,不过碍于本身是xml类型,能做的功能好像也只限于启动文件.内有一些标签可以添加配置信息,不过也是服务于ROS和连接设备这一方面的了.  运行一个launch文件ROS采用

> age_name launch_name && ros2 launch package_name launch_name (一般launch也是放在功能包中)     ROS2中 launch文件可能需要手动放置到install/../share相应功能包的里才能被找到

3. [工作空间]就是你项目的repository,也是你采用编译工具的目录.一般是工作空间中创建一个src文件夹来存放[功能包],每个*功能包*中又存放你的源文件和环境依赖信息(CMakelist和package),当你在工作空间目录编译(cmake或者colcon)后,当前目录会出现build,install,log目录,用来存放历史log和你的包配置信息.运行一个节点ROS采用 

> rosrun package_name node_name && ros2 run package_name node_name (如果是ROS2中的python文件,节点名要去setup.py中写)

4. [^rqt]: 可以用来查看你运行的节点连接情况

``rosrun rqt_graph_tree rqt_graph_tree && ros2 run rqt_graph_tree rqt_graph_tree ``

> 可根据当前内存中节点运行情况,生成连接图
