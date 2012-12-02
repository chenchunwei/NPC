using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public enum InstanceStatus
    {
        [Description("运行中")]
        Runing = 0,
        [Description("执行完成")]
        ActionCompleted = 1,
        [Description("处理完成")]
        Finished = 2

    }
}
