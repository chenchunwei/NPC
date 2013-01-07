using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NpcMmsSends
{
    public enum SendStatus
    {
        [Description("未处理")]
        Default = 0,
        [Description("已处理")]
        Done = 1,
        [Description("忽略")]
        Ignore = 2
    }
}
