using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Nodes;

namespace NPC.Application.MainTownModels
{
  public  class RightBarModel
    {
      public RightBarModel()
      {
          PublicProposals = new List<NodeRecord>();
      }

      public IList<NodeRecord> PublicProposals { get; set; }
      public Node PublicProposalsNode { get; set; }
    }
}
