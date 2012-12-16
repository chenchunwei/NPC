using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Articles;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Application.ManageModels.NodeRecords
{
    public class NodeRecordSearchModel
    {
        public NodeRecordSearchModel()
        {
            NodeRecordQueryItem = new NodeRecordQueryItem();
        }
        public NodeRecordQueryItem NodeRecordQueryItem { get; set; }
    }
}
