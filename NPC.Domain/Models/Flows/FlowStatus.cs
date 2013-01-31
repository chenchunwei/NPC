using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Flows
{
    [Flags]
    public enum FlowStatus
    {
        /// <summary>
        /// 实例化
        /// </summary>
        [Description("发起流程")]
        Instance = 1,
        /// <summary>
        /// 实例化
        /// </summary>
        [Description("任务开始")]
        Start = 2,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("执行中")]
        Running = 4,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("挂起")]
        Suspend = 8,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("手动停止")]
        Stop = 16,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("已完成")]
        Finished = 32,
    }
}
