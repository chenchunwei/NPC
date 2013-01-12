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
            NodeIds=new List<Guid>();
        }
        public Guid? NodeId { get; set; }
        public Guid? NodeIdLike { get; set; }
        public IList<Guid> NodeIds { get; set; }
        public string Keyword { get; set; }
        public bool? IsShow { get; set; }
        public Guid? UnitId { get; set; }
    }
}
