using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Application.MianModels
{
    public class SideBarModel
    {
        public SideBarModel()
        {
            Mediums = new List<NodeRecord>();
        }

        public NodeRecord ProposalNode { get; set; }
        public IList<NodeRecord> Mediums { get; set; }
    }
}
