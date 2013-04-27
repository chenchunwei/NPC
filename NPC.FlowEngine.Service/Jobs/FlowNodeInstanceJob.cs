using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Quartz;
using Quartz.Impl;
using log4net;
using FlowEngineServiceInEngine = NPC.FlowEngine.FlowEngineService;

namespace NPC.FlowEngine.Service.Jobs
{
    public class FlowNodeInstanceJob : IJob
    {
        private readonly ILog _logger;
        private readonly FlowEngineServiceInEngine _flowEngineService;
        public FlowNodeInstanceJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _flowEngineService = new FlowEngineServiceInEngine();
        }
        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在创建流程任务实例");
            _flowEngineService.CreateFlowNodeInstance();
            _logger.Info("实例创建完毕");
        }
    }
}
