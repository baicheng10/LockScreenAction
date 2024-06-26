# LockScreenAction
## 简介
windows锁屏后自动断开网络，解锁后恢复网络连接
## 功能
- 监听锁屏事件，禁用所有物理网络适配器
- 监听解锁事件，恢复所有物理网络适配器
- ~~监听锁屏事件，禁用配置文件中的物理网络适配器~~
- ~~监听解锁事件，恢复配置文件中的物理网络适配器~~
## 安装使用
- 以管理员权限运行，程序将自动最小化到任务栏托盘
- ~~新增Config.ini文件（注意编码）~~
```
[CONFIG]
;要禁用的物理网络适配器，名称使用逗号隔开
NETWORK_NAME ="以太网,以太网 2,以太网3"
```

## 注意事项
- 必须以管理员权限运行
- 目前仅可禁用物理网络适配器
- 可以使用ping -t 查看网络是否断开及恢复，以测试程序是否正常工作
- 注册表 HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run 新建字符串值，数值数据为exe可执行文件路径，exe文件兼容性设置为“以管理员权限运行”
