using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class ProposalBasicModel : FlowBasicModel
    {
        public Proposal Proposal { get; set; }
    }
}
