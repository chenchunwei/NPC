using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Proposals
{
    public enum ProposalType
    {
        [Description("无")]
        None = 0,
        [Description("意见建议")]
        NpcSuggest = 1,
        [Description("议案建议")]
        NpcProposal = 2
    }
}
