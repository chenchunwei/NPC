using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Flows
{
    public enum FlowStatus
    {
        /// <summary>
        /// 实例化
        /// </summary>
        [Description("发起流程")]
        Instance = 0,
        /// <summary>
        /// 实例化
        /// </summary>
        [Description("任务开始")]
        Start = 0,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("执行中")]
        Running = 1,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("挂起")]
        Suspend = 2,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("手动停止")]
        Stop = 3,
        /// <summary>
        /// 已打开
        /// </summary>
        [Description("已停止")]
        Finished = 4,
    }
}
