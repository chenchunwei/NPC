using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Application.MianModels.Homes
{
    public class RecordsModel
    {
        public RecordsModel()
        {
            NodeRecordQueryItem = new NodeRecordQueryItem();
            NodeRecords=new List<NodeRecord>();
        }
        public NodeRecordQueryItem NodeRecordQueryItem { get; set; }
        public List<NodeRecord> NodeRecords { get; set; }
        public string ListTitle { get; set; }
    }
}
