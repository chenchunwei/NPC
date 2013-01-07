using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Npc.Message.Job.Service
{
    public class ServiceEntrace
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new JobService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
