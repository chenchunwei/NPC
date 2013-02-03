using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Fluent.Infrastructure.Log;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using log4net;

namespace NPC.FlowEngine.Service
{
    public partial class FlowEngineService : ServiceBase
    {
        private readonly ILog _logger;
        private readonly JobEntrance _jobEntrance;
        public FlowEngineService()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _jobEntrance = new JobEntrance();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.Info("流程服务正在启动中……");
                _jobEntrance.Run();
                _logger.Info("流程服务正常启动");
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("流程服务启动异常：{0}", exception);
            }
            //Thread.Sleep(1000000);
        }

        protected override void OnStop()
        {
            _logger.Info("流程服务正在停止中……");
            _jobEntrance.Stop();
            _logger.Info("流程服务已停止");
        }
    }
}
