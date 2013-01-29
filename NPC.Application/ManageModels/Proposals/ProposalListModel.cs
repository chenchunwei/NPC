using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Proposals;

namespace NPC.Application.ManageModels.Proposals
{
    public class ProposalListModel
    {
        public ProposalListModel()
        {
            Proposals=new List<Proposal>();
            ProposalSearchModel=new ProposalSearchModel();
        }
        public ProposalSearchModel ProposalSearchModel { get; set; }

        public IList<Proposal> Proposals { get; set; }
    }
}
