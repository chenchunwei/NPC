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
        NpcAuditing = 1,
        [Description("人大常委会退回")]
        NpcSendBack = 2,
        [Description("市政办退回")]
        GovSendBack = 4,
        [Description("市政办处理中")]
        GovAuditing = 8,
        [Description("主办单位退回")]
        SponsorSendBack = 16,
        [Description("主办单位处理中")]
        SponsorAuditing = 32,
        [Description("不满意退回")]
        NpcAssessmentSendBack = 64,
        [Description("代表满意度回馈")]
        NpcAssessmenting = 128,
        [Description("完成")]
        Finished = 256,
        [Description("中止")]
        Stop = 512
    }
}
