using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class ScNpcAuditModel:ProposalBasicModel 
    {
        public string Comment { get; set; }
        public ScNpcAuditAction Action { get; set; }
    }
}
