using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels.Proposals
{
    public enum ScNpcAuditAction
    {
        [Description("提交市政办")]
        Submit = 0,
        [Description("不提交")]
        UnSubmit = 1,
    }
}
