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
        Npc = 0,
        [Description("群众议案")]
        Masses = 1
    }
}
