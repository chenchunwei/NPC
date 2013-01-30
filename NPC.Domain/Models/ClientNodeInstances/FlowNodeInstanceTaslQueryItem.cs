using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.ClientNodeInstances
{
    public class FlowNodeInstanceTaslQueryItem : QueryItemBase
    {
        public string FlowTypeName { get; set; }
        public Guid? UserId { get; set; }
        public string NodeName { get; set; }
    }
}
