using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.Flows
{
    public class FlowHistory
    {
        public FlowHistory()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Stage { get; set; }
        public virtual string Operator { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual string Comment { get; set; }
    }
}
