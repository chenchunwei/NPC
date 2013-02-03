using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Proposals
{
    [Flags]
    public enum NpcAssessmentState
    {
        [Description("满意")]
        Satisfy = 1,
        [Description("基本满意")]
        Normal = 2,
        [Description("不满意")]
        UnSatisfy = 4
    }
}
