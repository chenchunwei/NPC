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
        Runing = 1,
        [Description("执行完成")]
        ActionCompleted = 2,
        [Description("处理完成")]
        Finished = 4,
        /// <summary>
        /// 流程被设置成完成或跳转后,节点被设置成完成
        /// </summary>
        [Description("忽略")]
        Ignore = 8
    }
}
