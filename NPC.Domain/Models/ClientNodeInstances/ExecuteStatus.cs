using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public enum ExecuteStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [Description("待执行")]
        WaitingExecute = 0,
        /// <summary>
        /// 已执行
        /// </summary>
        [Description("已执行")]
        Executed = 1,
        /// <summary>
        /// 忽略
        /// </summary>
        [Description("忽略")]
        Ignore = 2
    }
}
