using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NodeRecords
{
    public class NodeRecordQueryItem : QueryItemBase
    {
        public NodeRecordQueryItem()
        {
            Pagination=new Pagination();
        }
        public Guid? NodeId { get; set; }
        public string Keyword { get; set; }
        public bool? IsShow { get; set; }
        public Guid? UnitId { get; set; }
    }
}
