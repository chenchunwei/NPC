using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Tasks
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        [Description("新创建")]
        New = 0,
        [Description("处理中")]
        Processing = 1,
        [Description("已完成")]
        Finished = 2,
        [Description("已取消")]
        Cancel = 3
    }
}
