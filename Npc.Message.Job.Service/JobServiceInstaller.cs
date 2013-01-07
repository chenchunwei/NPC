using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Npc.Message.Job.Service
{
    [RunInstaller(true)]
    public class JobServiceInstaller : Installer
    {
        readonly ServiceProcessInstaller _processInstall;
        readonly ServiceInstaller _serviceInstall;

        public JobServiceInstaller()
        {
            _processInstall = new ServiceProcessInstaller();
            _serviceInstall = new ServiceInstaller();
            _processInstall.Account = ServiceAccount.LocalSystem;
            _serviceInstall.ServiceName = "NpcMmsSendService";//System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
            _serviceInstall.Description = "用于人大在线系统发送彩信的宿主服务";
            Installers.Add(_serviceInstall);
            Installers.Add(_processInstall);
        }
    }
}
