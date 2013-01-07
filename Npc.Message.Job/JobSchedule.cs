using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npc.Message.Job.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Npc.Message.Job
{
    public class JobSchedule
    {
        private readonly IScheduler _scheduler;
        public JobSchedule()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler();
        }
        public void Start()
        {
            IJobDetail berthInUasgeJob = new JobDetailImpl("NpcMmsJob", "NpcMessageJob", typeof(NpcMmsJob));
            var trigger = new CronTriggerImpl("NpcMmsJobTrigger", "NpcMessageJob", "00 0/10 * * * ? *");

            _scheduler.ScheduleJob(berthInUasgeJob, trigger);
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.Shutdown();
        }
    }
}
