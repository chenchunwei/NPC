using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.FlowTypes
{
    public class FlowNodeLine
    {
        public FlowNodeLine()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual FlowNode ContactTo { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
