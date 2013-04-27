using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Quartz;
using log4net;
using FlowEngineServiceInEngine = NPC.FlowEngine.FlowEngineService;

namespace NPC.FlowEngine.Service.Jobs
{
    public class DealFlowNodeFlowToJob : IJob
    {
        private readonly ILog _logger;
        private readonly FlowEngineServiceInEngine _flowEngineService;
        public DealFlowNodeFlowToJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _flowEngineService = new FlowEngineServiceInEngine();
        }
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在处理流程流转");
            _flowEngineService.DealFlowNodeFlowTo();
            _logger.Info("处理流程流转完毕");
        }
    }
}

