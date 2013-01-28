using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Flows
{
    public class FlowQueryItem : QueryItemBase
    {
        public string FlowTypeName { get; set; }
        public string Title { get; set; }
        public Guid? UserId { get; set; }
        public string NodeName { get; set; }
    }
}
