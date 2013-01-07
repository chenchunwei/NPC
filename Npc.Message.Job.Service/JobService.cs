using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Fluent.Infrastructure.Log;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using log4net;

namespace Npc.Message.Job.Service
{
    public class JobService : ServiceBase
    {

        private readonly ILog _logger;
        private readonly JobSchedule _jobSchedule;
        public JobService()
        {
            var loggerFactory = new DefaultLoggerFactory();
            _logger = loggerFactory.GetLogger(this.GetType().ToString());
            _jobSchedule = new JobSchedule();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _jobSchedule.Start();
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("Job启动异常：{0}", exception);
            }
        }

        protected override void OnStop()
        {
            _jobSchedule.Stop();
            _logger.Info("Job停止");
        }
    }
}
