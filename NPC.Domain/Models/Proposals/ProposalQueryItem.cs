using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Proposals
{
    public class ProposalQueryItem : QueryItemBase
    {
        public virtual string Title { get; set; }
        public virtual ProposalStatus? ProposalStatus { get; set; }
    }
}
