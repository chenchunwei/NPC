using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npc.Message.Job.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Npc.Message.Job.Tests
{
    [TestClass]
    public class MmsJobTests
    {
        [TestMethod]
        public void TestMmsJob()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            var scheduler = sf.GetScheduler();


            IJobDetail berthInUasgeJob = new JobDetailImpl("berthInUsage", "IntelligentParking", typeof(NpcMmsJob));
            var trigger = new CronTriggerImpl("berthInUsageTrigger", "IntelligentParking", "00 0/5 * * * ? *");

            scheduler.ScheduleJob(berthInUasgeJob, trigger);
            scheduler.Start();

            scheduler.TriggerJob(new JobKey("berthInUsage", "IntelligentParking"));

            Thread.Sleep(1000000);
            scheduler.Clear();
            scheduler.Shutdown();

        }
    }
}
