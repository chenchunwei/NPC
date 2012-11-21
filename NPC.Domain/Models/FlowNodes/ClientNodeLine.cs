using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodes
{
    public class ClientNodeLine
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ClientNode Belongs { get; set; }
        public virtual ClientNode ContactTo { get; set; }
        public virtual string RuleCode { get; set; }
    }
}
