using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.FlowNodeInstances
{
    public class FlowNodeInstanceQueryItem : QueryItemBase
    {
        public string FlowTypeName { get; set; }
        public Guid? UserId { get; set; }
        public string ClientFlowNodeName { get; set; }
    }
}
