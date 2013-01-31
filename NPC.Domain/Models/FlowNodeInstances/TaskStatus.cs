using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodeInstances
{
    [Flags]
    public enum TaskStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [Description("创建")]
        Created = 1,
        /// <summary>
        /// 待执行
        /// </summary>
        [Description("已打开")]
        Opend = 2,
        /// <summary>
        /// 待执行
        /// </summary>
        [Description("执行中")]
        Executing = 4,
        /// <summary>
        /// 已执行
        /// </summary>
        [Description("已执行")]
        Executed = 8,
        /// <summary>
        /// 忽略
        /// </summary>
        [Description("忽略")]
        Ignore = 16
    }
}
