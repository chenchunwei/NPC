using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.FlowTypes
{
    public class ClientNodeLine
    {
        public ClientNodeLine()
        {
            RecordDescription=new RecordDescription();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ClientNode ContactTo { get; set; }
        public virtual string RuleCode { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}
