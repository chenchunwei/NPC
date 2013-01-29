using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class RequestViewModel
    {
        public Guid Id { get; set; }
        public Proposal Proposal { get; set; }
        public Flow Flow { get; set; }
    }
}
