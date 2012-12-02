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
    public class FlowNodeInstanceJob : IJob
    {
        private readonly IScheduler _scheduler;
        private readonly ILog _logger;
        private readonly FlowEngineService _flowEngineService;
        public FlowNodeInstanceJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _flowEngineService = new FlowEngineService();
        }
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在创建流程任务实例");
            _flowEngineService.CreateFlowNodeInstance();
            _flowEngineService.DealFlowNodeFlowTo();
            _logger.Info("实例创建完毕");
        }
    }
}
