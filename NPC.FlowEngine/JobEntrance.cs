using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            IJobDetail flowNodeInstanceJob = new JobDetailImpl("FlowNodeInstanceJob", "Npc", typeof(FlowNodeInstanceJob));
            IJobDetail dealFlowNodeFlowToJob = new JobDetailImpl("DealFlowNodeFlowToJob", "Npc", typeof(DealFlowNodeFlowToJob));
            IJobDetail dealFlowJob = new JobDetailImpl("DealFlowJob", "Npc", typeof(DealFlowJob));
            var flowNodeInstanceJobTrigger = new CronTriggerImpl("FlowNodeInstanceJobTrigger", "Npc", "0/15 * * * * ? *");
            var dealFlowNodeFlowToJobTrigger = new CronTriggerImpl("DealFlowNodeFlowToJobTrigger", "Npc", "0/15 * * * * ? *");
            var dealFlowJobTrigger = new CronTriggerImpl("DealFlowJobTrigger", "Npc", "0/15 * * * * ? *");
            _scheduler.ScheduleJob(flowNodeInstanceJob, flowNodeInstanceJobTrigger);
            _scheduler.ScheduleJob(dealFlowNodeFlowToJob, dealFlowNodeFlowToJobTrigger);
            _scheduler.ScheduleJob(dealFlowJob, dealFlowJobTrigger);
            _scheduler.Start();
            _logger.InfoFormat("Job已启动");
            //Thread.Sleep(1000000);
        }
        public void Stop()
        {
            _scheduler.Shutdown();
            _logger.InfoFormat("Job停止");
        }
    }
}
