using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using NPC.FlowEngine.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using log4net;

namespace NPC.FlowEngine
{
    public class JobEntrance
    {
        private readonly IScheduler _scheduler;
        private readonly ILog _logger;
        public JobEntrance()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = sf.GetScheduler();
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger();

        }
        public void Run()
        {
            IJobDetail clientNodeInstanceJob = new JobDetailImpl("ClientNodeInstanceJob", "Npc", typeof(ClientNodeInstanceJob));
            var trigger = new CronTriggerImpl("clientNodeInstanceJobTrigger", "Npc", "00 0/1 * * * ? *");
            _scheduler.ScheduleJob(clientNodeInstanceJob, trigger);
            _scheduler.Start();
        }
        public void Stop()
        {
            _scheduler.Shutdown();
        }
    }
}
