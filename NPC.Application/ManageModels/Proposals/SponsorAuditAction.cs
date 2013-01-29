using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Proposals
{
    public enum SponsorAuditAction
    {
        [Description("提交")]
        Finished = 0,
        [Description("退回市政办")]
        SendBack = 1
    }
}
