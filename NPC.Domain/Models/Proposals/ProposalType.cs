using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Proposals
{
    public enum ProposalType
    {
        [Description("代表议案")]
        NpcProposal = 0,
        [Description("代表建议")]
        NpcSuggest = 1,
        [Description("群众意见")]
        MassesOpinion = 2
    }
}
