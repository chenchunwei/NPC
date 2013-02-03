using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class SponsorAuditModel : ProposalBasicModel
    {
        public string Comment { get; set; }
        public string ReplyAttachment { get; set; }
        public SponsorAuditAction Action { get; set; }
    }
}
