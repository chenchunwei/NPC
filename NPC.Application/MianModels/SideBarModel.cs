using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Nodes;

namespace NPC.Application.MianModels
{
    public class SideBarModel
    {
        public SideBarModel()
        {
            Mediums = new List<NodeRecord>();
        }

        public Node NpcStaffEntryNode { get; set; }
        public NodeRecord ProposalNode { get; set; }
        public IList<NodeRecord> Mediums { get; set; }
    }
}
