using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Proposals
{
    public enum NpcAssessmentAction
    {
        [Description("满意")]
        Satisfy,
        [Description("不满意")]
        UnSatisfy
    }
}
