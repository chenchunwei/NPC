using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Application.Services;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Repository;
using Quartz;
using log4net;

namespace Npc.Message.Job.Jobs
{
    public class NpcMmsJob : IJob
    {
        private readonly ILog _logger;
        private readonly NpcMmsSendService _npcMmsSendService;
        public NpcMmsJob()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _npcMmsSendService = new NpcMmsSendService();
        }
        public void Execute(IJobExecutionContext context)
        {
            _npcMmsSendService.Execute();
        }
    }
}
