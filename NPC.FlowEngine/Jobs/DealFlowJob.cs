using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Quartz;
using log4net;

namespace NPC.FlowEngine.Jobs
{
    class DealFlowJob : IJob
    {
        private readonly ILog _logger;
        private readonly FlowEngineService _flowEngineService;
        public DealFlowJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _flowEngineService = new FlowEngineService();
        }
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在处理流程状态");
            _flowEngineService.DealFlow();
            _logger.Info("流程状态处理完毕");
        }
    }
}

 