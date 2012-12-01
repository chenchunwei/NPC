using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
            _logger = loggerFactory.GetLogger(this.GetType().ToString());
            _jobEntrance = new JobEntrance();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _jobEntrance.Run();
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("流程服务启动异常：{0}", exception);
            }
        }

        protected override void OnStop()
        {
            _jobEntrance.Stop();
            _logger.Info("流程服务已停止");
        }
    }
}
