using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.Articles;
using NPC.Domain.Models.Articles;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Application.ManageModels.NodeRecords
{
    public class NodeRecordListModel
    {
        public NodeRecordListModel()
        {
            NodeRecords = new List<NodeRecord>();
            NodeRecordSearchModel = new NodeRecordSearchModel();
        }
        public IList<NodeRecord> NodeRecords { get; set; }
        public NodeRecordSearchModel NodeRecordSearchModel { get; set; }
    }
}
