using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using NPC.Service;
using Quartz;
using log4net;
using FlowEngineServiceInEngine = NPC.FlowEngine.FlowEngineService;

namespace NPC.FlowEngine.Service.Jobs
{
    public class FetchProposalFromMessageJob : IJob
    {
        private readonly ILog _logger;
        private readonly ProposalService _proposalService;
        public FetchProposalFromMessageJob()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();
            _proposalService = new ProposalService();
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Info("流程引擎正在处理上行短信");
            _proposalService.FetchProposalFromMessage();
            _logger.Info("处理上行短信完毕");
        }
    }
}
