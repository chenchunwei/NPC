using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class ProposalSearchModel
    {
        public ProposalSearchModel()
        {
            ProposalQueryItem=new ProposalQueryItem();
            ProposalStatusesOptions = new Dictionary<string, string>();
        }

        public IDictionary<string,string> ProposalStatusesOptions { get; set; }
        public ProposalQueryItem ProposalQueryItem { get; set; }
    }
}
