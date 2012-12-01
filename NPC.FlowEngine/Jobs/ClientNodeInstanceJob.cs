using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Quartz;
using Quartz.Impl;
using log4net;

namespace NPC.FlowEngine.Jobs
{
    public class ClientNodeInstanceJob : IJob
    {
        private readonly IScheduler _scheduler;
        private readonly ILog _logger;
        public ClientNodeInstanceJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
         
        }
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在创建流程任务实例");

            _logger.Info("实例创建完毕");
        }
    }
}
