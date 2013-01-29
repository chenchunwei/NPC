using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Proposals
{
    [Flags]
    public enum ProposalStatus
    {
        [Description("默认")]
        Default = 0,
        [Description("人大常委会审核中")]
        NpcAudit = 1,
        [Description("市政办处理中")]
        GovAudit = 2,
        [Description("主办单位处理中")]
        SponsorAudit = 4,
        [Description("代表满意度回馈")]
        NpcAssessment = 8,
        [Description("完成")]
        Finished = 16,
        [Description("中止")]
        Stop = 32
    }
}
