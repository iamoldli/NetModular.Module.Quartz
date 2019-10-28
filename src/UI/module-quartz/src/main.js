import WebHost from 'netmodular-module-admin'
import config from './config'
import Quartz from './index'

// 注册模块
WebHost.registerModule(Quartz)

// 启动
WebHost.start(config)
