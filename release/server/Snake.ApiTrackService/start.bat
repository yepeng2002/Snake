@ECHO "请按任意键开始安装后台服务. . ."
@ECHO "清理原有服务项. . ."
Snake.ApiTrackService.exe  uninstall
@ECHO "清理完毕，开始安装后台服务. . ."
Snake.ApiTrackService.exe  install
@ECHO "服务安装完毕，启动服务. . ."
net start SnakeConsumer
@ECHO "服务启动成功！"
PAUSE