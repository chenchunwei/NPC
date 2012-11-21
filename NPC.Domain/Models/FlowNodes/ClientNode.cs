using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodes
{
    public class ClientNode
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}
