using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowTypes
{
    public class FlowDataField
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string DefaultValue { get; set; }
    }
}
