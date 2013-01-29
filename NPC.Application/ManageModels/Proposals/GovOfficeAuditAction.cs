using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Proposals
{
    public enum GovOfficeAuditAction
    {
        [Description("提交")]
        Submit = 0,
        [Description("退回人大常委")]
        SendBack = 1
    }
}
